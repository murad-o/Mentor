using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Jwt
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshTokenAsync(User user);
        Task SetRefreshTokenStatusToUsedAsync(RefreshToken refreshToken);
        Task RevokeRefreshTokenAsync(User user);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        bool IsTokenExpired(RefreshToken refreshToken);
    }
}
