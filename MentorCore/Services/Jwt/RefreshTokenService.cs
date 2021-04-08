using System;
using System.Threading.Tasks;
using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces.Jwt;
using Microsoft.EntityFrameworkCore;

namespace MentorCore.Services.Jwt
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        public RefreshTokenService(AppDbContext context, ITokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> CreateRefreshTokenAsync(User user)
        {
            var token = _tokenGenerator.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = token,
                ExpireTime = DateTime.Now.AddMonths(1),
                UserId = user.Id,
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task DeactivateRefreshTokenAsync(RefreshToken refreshToken)
        {
            refreshToken.Used = true;
            await _context.SaveChangesAsync();
        }

        public async Task RevokeRefreshTokensAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.UserName == username);

            _context.RemoveRange(user.RefreshTokens);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens.SingleOrDefaultAsync(t => t.Token == token);
        }

        public bool IsTokenExpired(RefreshToken refreshToken)
        {
            return refreshToken.ExpireTime <= DateTime.Now;
        }
    }
}
