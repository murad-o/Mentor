using MentorCore.Models.JWT;
using Microsoft.Extensions.Configuration;

namespace MentorCore.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfiguration GetJwtConfigurations(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>();
        }
    }
}
