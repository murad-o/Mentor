using System.Collections.Generic;
using System.Security.Claims;

namespace MentorCore.Interfaces.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
