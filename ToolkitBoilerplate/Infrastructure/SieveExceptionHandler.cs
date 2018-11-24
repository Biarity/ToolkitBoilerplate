using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sieve.Exceptions;
using System;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure
{
    public class SieveExceptionHandler
    {
        private readonly RequestDelegate _next;

        public SieveExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (SieveException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            object result;

            var code = 400;

            result = new
            {
                Error = "Filter/Sort/Paginate error."
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }

    public static class SieveExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseSieveExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SieveExceptionHandler>();
        }
    }

}
