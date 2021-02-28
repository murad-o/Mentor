using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces;
using MentorCore.Models.Email;
using MentorCore.Services.Account;
using MentorCore.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api.Extensions
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
            services.AddIdentity<User, IdentityRole>(options =>
                    options.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureRouting(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
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
            services.AddTransient<IRegisterService, RegisterService>();
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
