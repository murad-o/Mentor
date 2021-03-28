using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MentorCore.Interfaces.Jwt;
using MentorCore.Models.JWT;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Services.Jwt
{
    public class JwtExpiredTokenService : IJwtExpiredTokenService
    {
        private readonly JwtConfiguration _jwtConfigurations;

        public JwtExpiredTokenService(JwtConfiguration jwtConfigurations)
        {
            _jwtConfigurations = jwtConfigurations;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _jwtConfigurations.ValidIssuer,
                ValidAudience = _jwtConfigurations.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.SecretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
