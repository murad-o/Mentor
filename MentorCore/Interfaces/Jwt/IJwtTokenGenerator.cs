using MentorCore.DTO.Users;

namespace MentorCore.Interfaces.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(UserModel user);
        string GenerateRefreshToken();
    }
}
