using System.Collections.Generic;
using System.Security.Claims;

namespace MentorCore.Interfaces.Jwt
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
