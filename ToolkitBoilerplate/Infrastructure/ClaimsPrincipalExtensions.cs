using System;
using System.Linq;
using System.Security.Claims;

namespace ToolkitBoilerplate.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        // Will throw if unauthorized
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return Int32.Parse(user.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }

        public static string GetSecurityStamp(this ClaimsPrincipal user)
        {
            return user.Claims?.FirstOrDefault(c => c.Type == "AspNet.Identity.SecurityStamp")?.Value;
        }
    }
}
