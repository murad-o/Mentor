using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Users;
using AutoMapper;
using Contracts.Users;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Services.Jwt;

namespace Services.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenValidation _tokenValidation;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(IHttpContextAccessor httpContextAccessor, TokenValidation tokenValidation, UserManager<User> userManager, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenValidation = tokenValidation;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserModel> GetCurrentUser()
        {
            var accessToken = GetJwtAccessToken();

            var tokenValidationParameters = _tokenValidation.CreateTokenValidationParameters();

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new BadRequestException("Invalid token");

            var username = principal.Identity!.Name;
            var user = await _userManager.FindByEmailAsync(username);

            var userModel = _mapper.Map<UserModel>(user);

            return userModel;
        }

        private string GetJwtAccessToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
        }
    }
}
