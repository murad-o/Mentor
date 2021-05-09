using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Entities.Models;
using MentorCore.Interfaces.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Services.Jwt
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly TokenValidation _tokenValidation;
        private readonly UserManager<User> _userManager;

        public AccessTokenService(TokenValidation tokenValidation, UserManager<User> userManager)
        {
            _tokenValidation = tokenValidation;
            _userManager = userManager;
        }

        public async Task<User> GetUserFromAccessTokenAsync(string accessToken)
        {
            var tokenValidationParameters = _tokenValidation.CreateTokenValidationParameters();

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            var username = principal.Identity!.Name;
            var user = await _userManager.FindByEmailAsync(username);

            return user;
        }
    }
}
