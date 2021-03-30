using System.Text;
using MentorCore.Models.JWT;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Services.Jwt
{
    public class TokenValidation
    {
        private readonly JwtConfiguration _jwtConfigurations;

        public TokenValidation(JwtConfiguration jwtConfigurations)
        {
            _jwtConfigurations = jwtConfigurations;
        }

        public TokenValidationParameters CreateTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _jwtConfigurations.ValidIssuer,
                ValidAudience = _jwtConfigurations.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.SecretKey))
            };
        }
    }
}
