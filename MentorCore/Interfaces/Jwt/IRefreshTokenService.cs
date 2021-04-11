using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Jwt
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task<string> CreateRefreshTokenAsync(User user);
        Task RemoveRefreshTokenAsync(RefreshToken refreshToken);
        Task RemoveRefreshTokensAsync(string username);
        bool IsTokenExpired(RefreshToken refreshToken);
    }
}
