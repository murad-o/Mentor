﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MentorCore.Interfaces.Jwt;
using MentorCore.Models.JWT;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Services.Jwt
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtConfiguration _jwtConfigurations;

        public JwtTokenGenerator(JwtConfiguration jwtConfigurations)
        {
            _jwtConfigurations = jwtConfigurations;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                _jwtConfigurations.ValidIssuer,
                _jwtConfigurations.ValidAudience,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtConfigurations.LifeTime),
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