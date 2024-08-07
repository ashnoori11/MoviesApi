﻿using Application.Services.JwtTokenService.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Services.JwtTokenService;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfigurationRoot configuration;
    public JwtTokenService()
    {
        configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public async Task<(string Token, DateTime Expiration)> GenerateTokenAsync(string userName, CancellationToken cancellationToken, bool isAdmin)
    {
        var claims = new List<Claim>()
        {
            new Claim("email",userName),
            new Claim("role",isAdmin ? "admin":string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddDays(1);
        var token = new JwtSecurityToken(issuer: null,
            audience: null, claims, null, expiration, signingCredentials: creds);

        cancellationToken.ThrowIfCancellationRequested();

        return await Task.FromResult((new JwtSecurityTokenHandler().WriteToken(token), expiration));
    }
}
