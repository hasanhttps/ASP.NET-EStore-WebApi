﻿using System.Text;
using EStore.Domain.Helpers;
using System.Security.Claims;
using EStore.Application.Services;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using EStore.Domain.Entities.Concretes;
using Microsoft.Extensions.Configuration;

namespace EStore.Infrastructure.Services;

public class TokenService : ITokenService {

    // Fields

    private readonly IConfiguration _configuration;

    // Constructor

    public TokenService(IConfiguration configuration) {
        _configuration = configuration;
    }

    // Methods

    public string CreateAccessToken(User user) {

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var tokenDescription = new SecurityTokenDescriptor() {
            Expires = DateTime.UtcNow.AddMinutes(3),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),

            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Role, user.Role.RoleName!),
                new Claim(ClaimTypes.Email, user.Email!)
            })
        };


        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken? token = tokenHandler.CreateToken(tokenDescription);


        return tokenHandler.WriteToken(token);
    }

    public RefreshToken CreateRefreshToken() {

        var refreshToken = new RefreshToken() {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpireTime = DateTime.UtcNow.AddMinutes(30),
            CreateTime = DateTime.UtcNow
        };
        return refreshToken;
    }

    public RefreshToken CreateRepasswordToken() {

        var repasswordToken = new RefreshToken() {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpireTime = DateTime.UtcNow.AddMinutes(30),
            CreateTime = DateTime.UtcNow
        };
        return repasswordToken;
    }

    public RefreshToken CreateConfirmEmailToken() {

        var confirmEmailToken = new RefreshToken() {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpireTime = DateTime.UtcNow.AddMinutes(30),
            CreateTime = DateTime.UtcNow
        };
        return confirmEmailToken;
    }
}
