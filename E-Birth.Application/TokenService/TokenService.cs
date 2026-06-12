using E_Birth.Application.Configratin;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Birth.Application.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtSettings> _settings;
        private readonly UserManager<ApplicationUser> userManager;

        public TokenService(IOptions<JwtSettings> options,UserManager<ApplicationUser> userManager)
        {
            this._settings = options;
            this.userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Value.Issuer,
                audience: _settings.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.Value.ExpiresInMinites),
                signingCredentials: creds
            );
            cancellationToken.ThrowIfCancellationRequested();
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
