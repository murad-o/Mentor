using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Jwt
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshTokenAsync(User user);
        Task DeactivateRefreshTokenAsync(RefreshToken refreshToken);
        Task RevokeRefreshTokensAsync(string username);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        bool IsTokenExpired(RefreshToken refreshToken);
    }
}
