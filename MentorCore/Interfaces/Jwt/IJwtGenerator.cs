using System.Collections.Generic;
using System.Security.Claims;

namespace MentorCore.Interfaces.Jwt
{
    public interface IJwtGenerator
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
