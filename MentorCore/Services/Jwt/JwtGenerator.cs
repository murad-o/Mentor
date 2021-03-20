using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MentorCore.Interfaces.Jwt;
using MentorCore.Models.JWT;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Services.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtGenerator(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                _jwtConfiguration.ValidIssuer,
                _jwtConfiguration.ValidAudience,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtConfiguration.LifeTime),
                signingCredentials: signinCredentials
            );
   
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
