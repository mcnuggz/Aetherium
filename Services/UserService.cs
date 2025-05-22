using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Aetherium.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public int? GetUserId()
        {
            var ctx = _contextAccessor.HttpContext;
            var sessionUserId = ctx?.Session.GetInt32("UserId");

            // Skip cookie fallback in development to avoid Hot Reload conflicts
            var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (sessionUserId != null || isDev)
                return sessionUserId;

            if (ctx?.Request.Cookies.TryGetValue("UserId", out string userIdValue) == true &&
            int.TryParse(userIdValue, out int userId))
            {
                return userId;
            }

            return null;
        }

        public void SetUserId(int userId, bool rememberMe, string role)
        {
            var ctx = _contextAccessor?.HttpContext;
            if (ctx == null) { return; }
            if (rememberMe)
            {
                ctx.Response.Cookies.Append("UserId", userId.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    IsEssential = true,
                    HttpOnly = true
                });
            }
            else
            {
                ctx.Session.SetInt32("UserId", userId);
                ctx.Session.SetString("UserRole", role);
            }
        } 
    }
}
