using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Abstractions.Jwt;
using Contracts.Users;
using Microsoft.IdentityModel.Tokens;
using Services.Models.JWT;

namespace Services.Services.Jwt
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtConfiguration _jwtConfigurations;

        public JwtTokenGenerator(JwtConfiguration jwtConfigurations)
        {
            _jwtConfigurations = jwtConfigurations;
        }

        public string GenerateAccessToken(UserModel user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new ("Id", user.Id),
                new(ClaimTypes.Name, user.Email)
            };

            var tokenOptions = new JwtSecurityToken(
                _jwtConfigurations.ValidIssuer,
                _jwtConfigurations.ValidAudience,
                claims,
                expires: DateTime.Now.AddDays(_jwtConfigurations.LifeTime),
                signingCredentials: signinCredentials
            );
   
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
