using Aetherium.Data;
using Aetherium.Enums;
using Aetherium.Models;
using Aetherium.Models.ViewModels;
using Aetherium.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Aetherium.Controllers
{
    public class HomeController : Controller
    {
        private readonly SmtpEmailService _emailService;
        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;

        public HomeController(ApplicationDbContext context, SmtpEmailService emailService, UserService userService, PasswordService passwordService)
        {
            _context = context;
            _emailService = emailService;
            _userService = userService;
            _passwordService = passwordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token)) {
                return RedirectToAction("Login");
            }
            var model = new ResetPasswordViewModel { Email = email, Token = token };

            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string email, string token)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.EmailConfirmationToken == token);
            if (user == null || user.IsEmailConfirmed) { return RedirectToAction("EmailConfirmationFailed"); }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = string.Empty;
            user.TokenGeneratedAt = null;

            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            return RedirectToAction("CodeOfConduct");
        }

        [HttpGet]
        public IActionResult CodeOfConduct()
        {
            var userId = _userService.GetUserId();

            if (userId == null) { return RedirectToAction("Index"); }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null || user.AgreedToCoC) { return RedirectToAction("Create", "Character"); }
            return View();
        }

        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            ViewBag.Message = "Thanks for signing up! Please check your email to confirm your account before logging in.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LandingPageViewModel model)
        {
            ModelState.Clear();
            TryValidateModel(model.Login);

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var ipAddress = _userService.GetClientIP();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Login.Email);

            if (user == null || !_passwordService.Verify(model.Login.Password, user.PasswordHash)) {
                ModelState.AddModelError("", "Invalid login attempt");
                return View("Index", model);
            }

            if (!user.IsEmailConfirmed)
            {
                ModelState.AddModelError("", "You must confirm your email before logging in.");
                return View("Index", model);
            }

            if (user.IsIPBanned || user.LastKnownIP == ipAddress && user.IsIPBanned)
            {
                ModelState.AddModelError("", "User is IP Banned");
                return View("Index", model);
            }

            var suspensionMessage = _userService.CheckSuspensionStatus(user);
            if (suspensionMessage != null)
            {
                ModelState.AddModelError("", suspensionMessage);
            }

            _userService.SetUserId(user.Id, model.Login.RememberMe, user.Role.ToString());

            if (!user.AgreedToCoC)
            {
                return RedirectToAction("CodeOfConduct");
            }

            CharacterModel selectedCharacter = _userService.SelectLoginCharacter(user.Id);
            if (selectedCharacter == null)
            {
                return RedirectToAction("Create", "Character");
            }
            
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(LandingPageViewModel model) {

            ModelState.Clear();
            TryValidateModel(model.Register);

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            string ipAddress = _userService.GetClientIP();
            var error = _userService.ValidateRegistration(model.Register.Email, ipAddress);
            if (error != null) { 
                ModelState.AddModelError("", error);
            }

            string passwordHash = _passwordService.Hash(model.Register.Password);
            var token = UserService.GenerateSecureToken();

            var newUser = new UserModel
            {
                Email = model.Register.Email,
                PasswordHash = passwordHash,
                Role = UserRoleEnum.User,
                IsSuspended = false,
                IsIPBanned = false,
                SuspensionDuration = 0,
                LastKnownIP = ipAddress,
                EmailConfirmationToken = token,
                TokenGeneratedAt = DateTime.UtcNow,
                PasswordResetToken = string.Empty,
                PasswordResetTokenExpiration = null
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.Id);
            HttpContext.Session.SetString("UserRole", newUser.Role.ToString());

            var confirmUrl = Url.Action("ConfirmEmail", "Home", new { email = model.Register.Email, Token = token }, Request.Scheme) ?? "";
            _emailService.SendConfirmationEmail(model.Register.Email, confirmUrl);

            return RedirectToAction("RegisterConfirmation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null) return RedirectToAction("ForgotPasswordConfirmation");

            var token = UserService.GenerateSecureToken();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(12);
            _context.SaveChanges();

            var resetLink = Url.Action("ResetPassword", "Home", new { email = user.Email, token }, Request.Scheme) ?? "";
            _emailService.SendPasswordResetEmail(user.Email, resetLink);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model) 
        {
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

            if (user.PasswordResetTokenExpiration < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Your reset token has expired. Please request a new one.");
                return View(model);
            }
            _passwordService.SetNewPassword(user, model.NewPassword);
            user.PasswordResetToken = string.Empty;
            user.PasswordResetTokenExpiration = null;
            _context.SaveChanges();

            return RedirectToAction("ResetPasswordConfirmation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CodeOfConduct(bool agree)
        {
            var userId = _userService.GetUserId();
            if (userId == null || !agree) { return RedirectToAction("Index"); }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if(user == null) { return RedirectToAction("Index"); }

            user.AgreedToCoC = true;
            _context.SaveChanges();

            return RedirectToAction("Create", "Character");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}