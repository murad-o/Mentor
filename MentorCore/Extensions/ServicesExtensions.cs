using System.Text;
using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces.Email;
using MentorCore.Interfaces.Jwt;
using MentorCore.Models.Email;
using MentorCore.Models.JWT;
using MentorCore.Services.Email;
using MentorCore.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MentorCore.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            string dbConnection = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dbConnection));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfigurations = configuration.GetJwtConfigurations();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtConfigurations.ValidIssuer,
                    ValidAudience = jwtConfigurations.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.SecretKey))
                };
            });

            services.AddSingleton(typeof(JwtConfiguration), jwtConfigurations);
        }

        public static void RegisterJwtGenerator(this IServiceCollection services)
        {
            services.AddSingleton<IJwtGenerator, JwtGenerator>();
        }

        public static void RegisterSmtpConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfig = configuration
                .GetSection(nameof(SmtpConfiguration))
                .Get<SmtpConfiguration>();

            services.AddSingleton(smtpConfig);
        }

        public static void RegisterEmailConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration
                .GetSection(nameof(EmailConfiguration))
                .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);
        }

        public static void RegisterEmailSender(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
