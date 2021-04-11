using Entities.Models;

namespace MentorCore.Interfaces.Jwt
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
