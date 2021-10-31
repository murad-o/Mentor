using System.Text;
using Microsoft.IdentityModel.Tokens;
using Services.Models.JWT;

namespace Services.Services.Jwt
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
