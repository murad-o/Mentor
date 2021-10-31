using Api.Extensions;
using Api.Middlewares;
using Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Services.Automapper;

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
            services.ConfigureRouting();
            services.ConfigureControllers();

            services.ConfigureCors();
            services.ConfigureApiVersion();
            services.ConfigureSwagger();

            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);

            services.ConfigureJwt(Configuration);
            services.RegisterJwtServices();

            services.RegisterSmtpConfigurations(Configuration);
            services.RegisterEmailConfigurations(Configuration);

            services.RegisterEmailSender();
            services.RegisterCourseService();
            services.RegisterAccountService();
            services.RegisterUserService();
            services.AddTransient<StatusCodeExceptionHandlerMiddleware>();
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
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodeExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
