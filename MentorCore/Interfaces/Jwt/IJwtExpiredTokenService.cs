using System.Security.Claims;

namespace MentorCore.Interfaces.Jwt
{
    public interface IJwtExpiredTokenService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
    }
}
