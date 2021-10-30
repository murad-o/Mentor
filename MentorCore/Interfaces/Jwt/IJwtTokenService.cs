using System.Threading.Tasks;
using Entities.Models;
using MentorCore.DTO.Account;
using MentorCore.DTO.Users;

namespace MentorCore.Interfaces.Jwt
{
    public interface IJwtTokenService
    {
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task<string> CreateRefreshTokenAsync(UserModel user);
        Task<JwtTokenModel> UpdateJwtTokenAsync(JwtTokenModel jwtTokenModel);
        Task RemoveRefreshTokenAsync(RefreshToken refreshToken);
        Task RemoveRefreshTokensAsync();
    }
}
