using Contracts.Users;

namespace Abstractions.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(UserModel user);
        string GenerateRefreshToken();
    }
}
