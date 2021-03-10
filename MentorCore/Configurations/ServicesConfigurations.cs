using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces.Email;
using MentorCore.Models.Email;
using MentorCore.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MentorCore.Configurations
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
            .AddDefaultTokenProviders();
        }

        public static void ConfigureSmtp(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfig = configuration
                .GetSection(nameof(SmtpConfiguration))
                .Get<SmtpConfiguration>();

            services.AddSingleton(smtpConfig);
        }

        public static void ConfigureEmail(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration
                .GetSection(nameof(EmailConfiguration))
                .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);
        }

        public static void AddOwnServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
