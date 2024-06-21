using System.Text;
using EStore.Domain.DTOs;
using EStore.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using EStore.Application.Services;
using System.Security.Cryptography;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;

namespace EStore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {

    // Fields

    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IReadRoleRepository _readRoleRepository;
    private readonly IReadUserRepository _readUserRepository;
    private readonly IWriteUserRepository _writeUserRepository;
    private readonly IWriteUserTokenRepository _writeUserTokenRepository;

    // Constructor

    public AuthController(
        IEmailService emailService, 
        ITokenService tokenService,
        IReadUserRepository readUserRepository, 
        IReadRoleRepository readRoleRepository,
        IWriteUserRepository writeUserRepository,
        IWriteUserTokenRepository writeUserTokenRepository) {

        _emailService = emailService;
        _tokenService = tokenService;
        _readRoleRepository = readRoleRepository;
        _readUserRepository = readUserRepository;
        _writeUserRepository = writeUserRepository;
        _writeUserTokenRepository = writeUserTokenRepository;
    }

    // Methods

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) {

        var user = await _readUserRepository.GetUserByUserName(loginDTO.UserName);
        if (user is null)
            return BadRequest("Invalid username");

        if (user.ConfirmEmail == false)
            return BadRequest("Email not confirmed");

        using var hmac = new HMACSHA256(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

        var isPasswordMatch = computedHash.SequenceEqual(user.PasswordHash);
        if (!isPasswordMatch)
            return BadRequest("Invalid password");

        var accessToken = _tokenService.CreateAccessToken(user);

        var refreshToken = _tokenService.CreateRefreshToken();
        SetRefreshToken(user, refreshToken);

        return Ok(new { accessToken = accessToken });
    }


    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken() {

        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("Invalid refresh token");

        var user = await _readUserRepository.GetUserByRefreshToken(refreshToken);
        if (user is null)
            return BadRequest("Invalid refresh token");

        var accessToken = _tokenService.CreateAccessToken(user);

        var refreshTokenObj = _tokenService.CreateRefreshToken();
        SetRefreshToken(user, refreshTokenObj);

        return Ok(new { accessToken = accessToken });
    }



    // Helper Method. SetRefreshToken
    private void SetRefreshToken(User user, RefreshToken refreshToken) {

        var cookieOptions = new CookieOptions() {
            HttpOnly = true,
            Expires = refreshToken.ExpireTime
        };

        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        if (user.UserToken == null) {
            var newtoken = new UserToken();
            newtoken.UserId = user.Id;
            _writeUserTokenRepository.AddAsync(newtoken);
            user.UserToken = newtoken;
            user.UserTokenId = newtoken.Id;
        }

        user.UserToken.RefreshToken = refreshToken.Token;
        user.UserToken.RefreshTokenCreateTime = refreshToken.CreateTime;
        user.UserToken.RefreshTokenExpireTime = refreshToken.ExpireTime;

        _writeUserRepository.UpdateAsync(user);
        _writeUserRepository.SaveChangeAsync();
    }


    // Register Method
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] UserDTO UserDTO) {

        var user = await _readUserRepository.GetUserByUserName(UserDTO.UserName);
        if (user is not null)
            return BadRequest("User already exists");

        var role = await _readRoleRepository.GetByRoleName(UserDTO.Role);

        var newUserToken = new UserToken();
        await _writeUserTokenRepository.AddAsync(newUserToken);

        using var hmac = new HMACSHA256();


        var newUser = new User() {
            UserName = UserDTO.UserName,
            Email = UserDTO.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(UserDTO.Password)),
            PasswordSalt = hmac.Key,
            RoleId = role!.Id,
            Role = role,
            ConfirmEmail = false,
            UserTokenId = newUserToken.Id,
            UserToken = newUserToken
        };

        var confirmEmailToken = _tokenService.CreateConfirmEmailToken();
        var actionUrl = $@"https://localhost:5046/api/Auth/ConfirmEmail?token={confirmEmailToken.Token}";
        var result = await _emailService.sendMailAsync(UserDTO.Email, "Confirm Your Email", $"Reset your password by <a href='{actionUrl}'>clicking here</a>.", true);

        newUser.UserToken.ConfirmEmailToken = confirmEmailToken.Token;
        newUser.UserToken.ConfirmEmailTokenCreateTime = confirmEmailToken.CreateTime;
        newUser.UserToken.ConfirmEmailTokenExpireTime = confirmEmailToken.ExpireTime;

        await _writeUserRepository.AddAsync(newUser);
        await _writeUserRepository.SaveChangeAsync();
        return Ok(new { ActionUrl = actionUrl});
    }

    // Confirm Email Method
    [HttpPost("[action]")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token) {

        var user = await _readUserRepository.GetUserByConfirmEmailToken(token);
        if (user is null)
            return BadRequest("User not found");

        if (user.UserToken.ConfirmEmailTokenExpireTime < DateTime.UtcNow)
            return BadRequest("ConfirmEmailToken expired");

        user.ConfirmEmail = true;

        await _writeUserRepository.UpdateAsync(user);
        await _writeUserRepository.SaveChangeAsync();
        return Ok("Email Confirmed");
    }

    // Forgot Password
    [HttpPost("[action]")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO) {

        var user = await _readUserRepository.GetUserByEmail(forgotPasswordDTO.Email);
        if (user is null)
            return BadRequest("User not found");

        var repasswordToken = _tokenService.CreateRepasswordToken();
        var actionUrl = $@"https://localhost:5046/api/Auth/ResetPassword?token={repasswordToken.Token}";
        var result = await _emailService.sendMailAsync(forgotPasswordDTO.Email, "Reset Your Password", $"Reset your password by <a href='{actionUrl}'>clicking here</a>.", true);

        user.UserToken.RePasswordToken = repasswordToken.Token;
        user.UserToken.RePasswordTokenCreateTime = repasswordToken.CreateTime;
        user.UserToken.RePasswordTokenExpireTime = repasswordToken.ExpireTime;

        await _writeUserRepository.UpdateAsync(user);
        await _writeUserRepository.SaveChangeAsync();

        return Ok(new { actionUrl = actionUrl });
    }

    // Reset Password
    [HttpPost("[action]")]
    public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromBody] ResetPasswordDTO resetPasswordDTO) {
     
        var user = await _readUserRepository.GetUserByRePasswordToken(token);
        if (user is null)
            return BadRequest("Invalid RePasswordToken");

        if (user.UserToken.RePasswordTokenExpireTime < DateTime.UtcNow)
            return BadRequest("RePasswordToken expired");

        using var hmac = new HMACSHA256();

        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDTO.Password));
        user.PasswordSalt = hmac.Key;

        await _writeUserRepository.UpdateAsync(user);
        await _writeUserRepository.SaveChangeAsync();
        return Ok();
    }
}
