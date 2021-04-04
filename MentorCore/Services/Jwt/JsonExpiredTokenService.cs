using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MentorCore.Interfaces.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Services.Jwt
{
    public class JsonExpiredTokenService : IJsonExpiredTokenService
    {
        private readonly TokenValidation _tokenValidation;

        public JsonExpiredTokenService(TokenValidation tokenValidation)
        {
            _tokenValidation = tokenValidation;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
        {
            var tokenValidationParameters = _tokenValidation.CreateTokenValidationParameters();

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
