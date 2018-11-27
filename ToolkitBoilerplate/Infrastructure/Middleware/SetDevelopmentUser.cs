using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ToolkitBoilerplate.Infrastructure.Middleware
{
    public class SetDevelopmentUser
    {
        private readonly RequestDelegate _next;

        public SetDevelopmentUser(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var devUserId = httpContext.Request.Query["dev_user_id"].ToString();
            var devUserName = httpContext.Request.Query["dev_user_name"].ToString();

            devUserId = String.IsNullOrEmpty(devUserId) ? "2" : devUserId;
            devUserName = String.IsNullOrEmpty(devUserName) ? "test_user_name" : devUserName;

            var identity = new ClaimsIdentity("DevAuth");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, devUserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, devUserName));
            identity.AddClaim(new Claim("AspNet.Identity.SecurityStamp", "1234567890"));

            httpContext.User = new ClaimsPrincipal(identity);

            await _next(httpContext);
        }

    }

    public static class SetDevelopmentUserExtensions
    {
        public static IApplicationBuilder UseSetDevelopmentUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetDevelopmentUser>();
        }
    }

}
