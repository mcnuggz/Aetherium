using Aetherium.Data;
using Aetherium.Enums;
using Aetherium.Models;
using Aetherium.Models.ViewModels;
using Aetherium.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Aetherium.Controllers
{
    public class HomeController : Controller
    {
        private readonly SmtpEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, SmtpEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel { Email = email, Token = token };
            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string email, string token)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.EmailConfirmationToken == token);
            if (user == null || user.IsEmailConfirmed)
            {
                return RedirectToAction("EmailConfirmationFailed");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.TokenGeneratedAt = null;

            _context.SaveChanges();
            return RedirectToAction("EmailConfirmed");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var ipAddress = GetClientIP();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)) {
                ModelState.AddModelError("", "Invalid login attempt");
                return View("Index", model);
            }

            // IP Ban check
            if (user.IsIPBanned || user.LastKnownIP == ipAddress && user.IsIPBanned)
            {
                ModelState.AddModelError("", "User is IP Banned");
                return View("Index", model);
            }

            if (user.IsSuspended)
            {
                if (user.SuspensionExpirationDate.HasValue && user.SuspensionExpirationDate.Value > DateTime.UtcNow)
                {
                    var remaining = user.SuspensionExpirationDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError(string.Empty, $"Account suspended. Time remaining: {remaining.Hours}h {remaining.Minutes}m.");
                    return View(model);
                }
                else
                {
                    user.IsSuspended = false;
                    user.SuspensionExpirationDate = null;
                    user.SuspensionDuration = 0;
                    _context.SaveChanges();
                }
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model) {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string ipAddress = GetClientIP();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            int registrationsToday = _context.Users.Count(u => u.LastKnownIP == ipAddress && u.CreatedAt.Date == DateTime.UtcNow.Date);
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // Checking for IP bans
            if (_context.Users.Any(u => u.IsIPBanned && u.LastKnownIP == ipAddress)) {
                ModelState.AddModelError(string.Empty, "Account Access or Registrations from your IP address are currently blocked. If you feel like this is incorrect, please email administration or reach out to us on Discord.");
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError(string.Empty, "Email or Username is already taken.");
                return View(model);
            }

            if (registrationsToday >= 3)
            {
                ModelState.AddModelError("", "Too many registrations from this IP today. Try again later.");
                return View(model);
            }

            var newUser = new UserModel
            {
                Email = model.Email,
                PasswordHash = passwordHash,
                Role = UserRoleEnum.User,
                IsSuspended = false,
                IsIPBanned = false,
                SuspensionDuration = 0,
                LastKnownIP = ipAddress,
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.Id);
            HttpContext.Session.SetString("UserRole", newUser.Role.ToString());

            var confirmUrl = Url.Action("ConfirmEmail", "Home", new { email = model.Email, Token = token }, Request.Scheme);
            _emailService.SendEmail(newUser.Email, "Confirm Your Account", $"<div style='text-align: center;'>Please confirm your account by clicking this link: <a href='{confirmUrl}'>Confirm Email</a><br /></div>");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null) 
                return RedirectToAction("ForgotPasswordConfirmation");
            
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(12);
            _context.SaveChanges();

            var resetLink = Url.Action("ResetPassword", "Home", new { email = user.Email, token = token }, Request.Scheme);
            var message = $"<p>Click the link to reset your password: <a href='{resetLink}'>Reset Password</a></p><p>This link will expire in <strong>12 hours</strong> for your security.</p>" ;
            _emailService.SendEmail(user.Email, "Reset Your Password", message);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model){
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _context.Users.FirstOrDefault(u =>u.Email == model.Email 
                && u.PasswordResetToken == model.Token 
                && u.PasswordResetTokenExpiration > DateTime.UtcNow);

            if(user == null)
            {
                ModelState.AddModelError("", "Invalid or expired token.");
                return View(model);
            }

            var hasher = new PasswordHasher<UserModel>();
            user.PasswordHash = hasher.HashPassword(user, model.Password);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiration = null;
            _context.SaveChanges();

            return RedirectToAction("ResetPasswordConfirmation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Private Methods
        private string GetClientIP()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
        #endregion
    }
}
