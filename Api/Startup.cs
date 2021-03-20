using Api.Extensions;
using MentorCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MentorCore.Services.Automapper;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDatabase(Configuration);

            services.ConfigureIdentity();

            services.ConfigureJwt(Configuration);

            services.RegisterJwtGenerator();

            services.ConfigureRouting();

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.ConfigureCors();

            services.ConfigureApiVersion();

            services.ConfigureSwagger();

            services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);

            services.RegisterSmtpConfigurations(Configuration);

            services.RegisterEmailConfigurations(Configuration);

            services.RegisterEmailSender();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
