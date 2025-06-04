using Aetherium.Data;
using Aetherium.Models;
using System.Security.Cryptography;

namespace Aetherium.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;
        public UserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _contextAccessor = httpContextAccessor;
            _context = context;
        }
        public int? GetUserId()
        {
            var ctx = _contextAccessor.HttpContext;
            var sessionUserId = ctx?.Session.GetInt32("UserId");

            if (sessionUserId != null)
            {
                return sessionUserId;
            }
                

            if (ctx?.Request.Cookies.TryGetValue("UserId", out string? userIdValue) == true && int.TryParse(userIdValue, out int userId))
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

        public string? CheckSuspensionStatus(UserModel user)
        {
            if (!user.IsSuspended) return null;
            if (user.IsSuspended)
            {
                if (user.SuspensionExpirationDate.HasValue && user.SuspensionExpirationDate.Value > DateTime.UtcNow)
                {
                    var remaining = user.SuspensionExpirationDate.Value - DateTime.UtcNow;
                    return $"Account suspended. Time remaining: {remaining.Days}days, {remaining.Hours}hours, and {remaining.Minutes}minutes.";
                }

                // Unsuspend
                user.IsSuspended = false;
                user.SuspensionExpirationDate = null;
                user.SuspensionDuration = 0;
                _context.SaveChanges();

            }
            return null;
        }

        public string? ValidateRegistration(string email, string ip)
        {
            if (_context.Users.Any(u => u.IsIPBanned && u.LastKnownIP == ip))
            {
                return "Account Access or Registrations from your IP address are currently blocked.";
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                return "Email is already being used.";
            }

            int registrationsToday = _context.Users.Count(u => u.LastKnownIP == ip && u.CreatedAt.Date == DateTime.UtcNow.Date);
            if (registrationsToday >= 3)
            {
                return "Too many registrations from this IP today. Try again later.";
            }

            return null;
        }

        public CharacterModel? SelectLoginCharacter(int userId)
        {
            var characters = _context.Characters
                .Where(c => c.UserAccountId == userId && !c.IsArchived)
                .OrderByDescending(c => c.LastLoggedIn)
                .ToList();

            if (!characters.Any()) return null;

            var selected = characters.First();
            selected.LastLoggedIn = DateTime.UtcNow;
            _context.SaveChanges();

            _contextAccessor.HttpContext?.Session.SetInt32("CharacterId", selected.Id);

            return selected;
        }

        public static string GenerateSecureToken(int length = 64)
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
        }

        public string GetClientIP()
        {
            return _contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        public void ClearUserSession()
        {
            var session = _contextAccessor.HttpContext?.Session;
            if (session != null) { 
                session.Clear();
            }
        }

        public string GetLoggedInCharacterAvatarUrl(int userId)
        {
            var character = _context.Characters
                .Where(c => c.UserAccountId == userId && !c.IsArchived && !string.IsNullOrWhiteSpace(c.AvatarUrl))
                .FirstOrDefault();

            return character?.AvatarUrl;
            
        }
    }
}
