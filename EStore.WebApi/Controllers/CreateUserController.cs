using System.Text;
using EStore.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using EStore.Domain.ViewModels;
using System.Security.Cryptography;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;

namespace EStore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CreateUserController : ControllerBase {

    // Fields

    private readonly IReadRoleRepository _readRoleRepository;
    private readonly IReadUserRepository _readUserRepository;
    private readonly IWriteUserRepository _writeUserRepository;
    private readonly IWriteUserTokenRepository _writeUserTokenRepository;

    // Constructor

    public CreateUserController(IReadRoleRepository readRoleRepository, IReadUserRepository readUserRepository, IWriteUserRepository writeUserRepository, IWriteUserTokenRepository writeUserTokenRepository) {
        _readRoleRepository = readRoleRepository;
        _readUserRepository = readUserRepository;
        _writeUserRepository = writeUserRepository;
        _writeUserTokenRepository = writeUserTokenRepository;
    }

    // Methods

    [HttpPost("[action]/{role}")]
    public async Task<IActionResult> AddUser(string role, [FromQuery] string accesstoken, [FromBody] UserDTO userDTO) {
        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Admin") {

            var result = await _readRoleRepository.GetByRoleName(role);
            if (result == null)
                return NotFound("This role was not found");

            if (result.RoleName == "SuperAdmin")
                return BadRequest("SuperAdmin cannot create");
            else if (result.RoleName == "Admin" && user.Role.RoleName == "Admin")
                return BadRequest("Admin can't create admin");

            var alreadyExists = await _readUserRepository.GetUserByUserName(userDTO.UserName);
            if (alreadyExists is not null)
                return BadRequest("User already exists");

            var newUserToken = new UserToken();
            await _writeUserTokenRepository.AddAsync(newUserToken);

            using var hmac = new HMACSHA256();

            var newUser = new User() {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password)),
                PasswordSalt = hmac.Key,
                RoleId = result!.Id,
                Role = result,
                UserTokenId = newUserToken.Id,
                ConfirmEmail = true,
            };

            await _writeUserRepository.AddAsync(newUser);
            return Ok($"User added with {role} role.");
        }
        else return Unauthorized("You don't have access to this operation!");

    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll([FromQuery] string accesstoken) {
        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Admin") {

            var result = await _readUserRepository.GetAllAsync();

            if (user.Role.RoleName == "SuperAdmin") {
                var allUsersVM = result.Where(p => p.Role.RoleName != "SuperAdmin").Select(p => new GetUserVM() {
                    FirstName = p.FirstName, 
                    LastName = p.LastName,
                    Address = p.Address,
                    Email = p.Email,
                    UserName = p.UserName,
                    PasswordHash = p.PasswordHash,
                    PasswordSalt = p.PasswordSalt,
                    ConfirmEmail = p.ConfirmEmail,
                    RoleId = p.RoleId,
                    UserTokenId = p.UserTokenId
                });
                return Ok(allUsersVM);
            }
            else {
                var allUsersVM = result.Where(p => p.Role.RoleName != "Admin").Select(p => new GetUserVM() {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Address = p.Address,
                    Email = p.Email,
                    UserName = p.UserName,
                    PasswordHash = p.PasswordHash,
                    PasswordSalt = p.PasswordSalt,
                    ConfirmEmail = p.ConfirmEmail,
                    RoleId = p.RoleId,
                    UserTokenId = p.UserTokenId
                }).ToList();
                return Ok(allUsersVM);
            }

        }
        else return Unauthorized("You don't have access to this operation!");
    }
}
