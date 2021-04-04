using System.Security.Claims;

namespace MentorCore.Interfaces.Jwt
{
    public interface IJsonExpiredTokenService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
    }
}
