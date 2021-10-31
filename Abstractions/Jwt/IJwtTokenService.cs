using System.Threading.Tasks;
using Contracts.Account;
using Contracts.Users;

namespace Abstractions.Jwt
{
    public interface IJwtTokenService
    {
        Task<string> CreateRefreshTokenAsync(UserModel user);
        Task<JwtTokenModel> UpdateJwtTokenAsync(JwtTokenModel jwtTokenModel);
        Task RemoveRefreshTokenAsync(string token);
        Task RemoveRefreshTokensAsync();
    }
}
