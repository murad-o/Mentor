using System;
using FluentValidation.AspNetCore;
using MentorCore.DTO.Validators;
using MentorCore.DTO.Validators.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Api.Extensions
{
    public static class ServicesExtensions
    {
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

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<LoginModelValidator>());
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mentor API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        },
                        Array.Empty<string>()
                    }
                });
                c.AddFluentValidationRules();
            });
        }
    }
}
