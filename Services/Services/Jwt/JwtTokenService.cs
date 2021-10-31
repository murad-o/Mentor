using System;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Jwt;
using Abstractions.Users;
using Contracts.Account;
using Contracts.Users;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Services.Services.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;

        public JwtTokenService(IJwtTokenGenerator jwtTokenGenerator, AppDbContext dbContext,
            IUserService userService)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<JwtTokenModel> UpdateJwtTokenAsync(JwtTokenModel jwtTokenModel)
        {
            var user = await _userService.GetCurrentUser();

            var refreshToken = await _dbContext.RefreshTokens
                .SingleOrDefaultAsync(t => t.Token == jwtTokenModel.RefreshToken);

            if (refreshToken is null || refreshToken.UserId != user.Id
                                     || IsTokenExpired(refreshToken))
                throw new BadRequestException("Invalid refresh token");

            _dbContext.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();

            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var newRefreshToken = await CreateRefreshTokenAsync(user);

            return new JwtTokenModel {AccessToken = newAccessToken, RefreshToken = newRefreshToken};
        }

        public async Task RemoveRefreshTokenAsync(string token)
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

            if (refreshToken is not null)
            {
                _dbContext.Remove(refreshToken);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveRefreshTokensAsync()
        {
            var user = await _userService.GetCurrentUser();

            var refreshTokens = await _dbContext.RefreshTokens
                .Where(x => x.UserId == user.Id).ToListAsync();

            _dbContext.RemoveRange(refreshTokens);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> CreateRefreshTokenAsync(UserModel user)
        {
            var token = _jwtTokenGenerator.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = token,
                ExpireTime = DateTime.Now.AddMonths(1),
                UserId = user.Id,
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return refreshToken.Token;
        }

        private static bool IsTokenExpired(RefreshToken refreshToken)
        {
            return refreshToken.ExpireTime <= DateTime.Now;
        }
    }
}
