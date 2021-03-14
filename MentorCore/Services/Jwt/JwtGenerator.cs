using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MentorCore.Extensions;
using MentorCore.Interfaces.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Services.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken()
        {
            var jwtConfigurations = _configuration.GetJwtConfigurations();
            
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtConfigurations.ValidIssuer,
                audience: jwtConfigurations.ValidAudience,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(jwtConfigurations.LifeTime),
                signingCredentials: signinCredentials
            );
   
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
