using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Jwt
{
    public interface IJwtRefreshTokenService
    {
        Task<string> CreateRefreshAndExpiryTokenAsync(User user);
        Task<string> UpdateRefreshTokenAsync(User user);
        Task RevokeRefreshTokenAsync(User user);
    }
}
