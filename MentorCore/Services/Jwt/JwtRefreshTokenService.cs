using System;
using System.Threading.Tasks;
using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces.Jwt;

namespace MentorCore.Services.Jwt
{
    public class JwtRefreshTokenService : IJwtRefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        
        public JwtRefreshTokenService(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> CreateRefreshAndExpiryTokenAsync(User user)
        {
            user.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            return user.RefreshToken;
        }

        public async Task<string> UpdateRefreshTokenAsync(User user)
        {
            user.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            await _context.SaveChangesAsync();
            return user.RefreshToken;
        }

        public async Task RevokeRefreshTokenAsync(User user)
        {
            user.RefreshToken = null;
            await _context.SaveChangesAsync();
        }
    }
}
