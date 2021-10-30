using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces.Account;
using MentorCore.Interfaces.Courses;
using MentorCore.Interfaces.Email;
using MentorCore.Interfaces.Jwt;
using MentorCore.Interfaces.Users;
using MentorCore.Models.Email;
using MentorCore.Models.JWT;
using MentorCore.Services.Account;
using MentorCore.Services.Courses;
using MentorCore.Services.Email;
using MentorCore.Services.Jwt;
using MentorCore.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfigs = configuration.GetJwtConfigurations();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidation(jwtConfigs).CreateTokenValidationParameters();
            });

            services.AddSingleton(typeof(JwtConfiguration), jwtConfigs);
        }

        public static void RegisterJwtServices(this IServiceCollection services)
        {
            services.AddSingleton<TokenValidation>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
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

        public static void RegisterCourseService(this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
        }

        public static void RegisterAccountService(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
        }

        public static void RegisterUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
