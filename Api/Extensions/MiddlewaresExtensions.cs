using Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Api.Extensions
{
    public static class MiddlewaresExtensions
    {
        public static IApplicationBuilder UseStatusCodeExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StatusCodeExceptionHandlerMiddleware>();
        }
    }
}
