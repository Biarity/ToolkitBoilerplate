using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure
{
    public class AddUserDetailCookie
    {
        private readonly RequestDelegate _next;

        public AddUserDetailCookie(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
                httpContext.Response.Cookies.Append("User", httpContext.User.GetUserName());
            else if (httpContext.Request.Cookies.ContainsKey("User"))
                httpContext.Response.Cookies.Delete("User");

            await _next(httpContext);
        }

    }

    public static class AddUserDetailCookieExtensions
    {
        public static IApplicationBuilder AddUserDetailCookie(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AddUserDetailCookie>();
        }
    }

}
