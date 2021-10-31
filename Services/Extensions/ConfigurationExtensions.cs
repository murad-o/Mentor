using Microsoft.Extensions.Configuration;
using Services.Models.JWT;

namespace Services.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfiguration GetJwtConfigurations(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>();
        }
    }
}
