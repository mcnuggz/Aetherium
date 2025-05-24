using Aetherium.Data;
using Aetherium.Models;
using Aetherium.Models.ViewModels;
using Aetherium.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Aetherium.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        public CharacterController(ApplicationDbContext applicationDbContext, UserService userService) {
            _context = applicationDbContext;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewByCustomUrl(string customUrl)
        {
            var character = _context.Characters
                .Include(c => c.UserAccount)
                .FirstOrDefault(c => c.CustomUrl == customUrl);

            if (character == null)
                return NotFound();

            return View("CharacterProfile", character);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = _userService.GetUserId();
            if (userId == null) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [HttpPost]
        public IActionResult Create(CharacterCreateViewModel model, IFormFile BannerFile)
        {
            var userId = _userService.GetUserId();
            if (userId == null) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? avatarPath = null;
            string? bannerPath = null;
            bool isBannerGif = false;

            if (!string.IsNullOrEmpty(model.CroppedAvatarBase64))
            {
                var base64Data = Regex.Match(model.CroppedAvatarBase64, @"data:image/(?<type>.+?);base64,(?<data>.+)").Groups["data"].Value;
                var fileBytes = Convert.FromBase64String(base64Data);

                var fileName = $"avatar_{Guid.NewGuid()}.png";
                var filePath = Path.Combine("wwwroot", "uploads", "avatars", fileName);
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
                System.IO.File.WriteAllBytes(filePath, fileBytes);

                avatarPath = $"/uploads/avatars/{fileName}";
            }

            if (BannerFile != null && BannerFile.Length > 0)
            {
                var extension = Path.GetExtension(BannerFile.FileName).ToLower();
                isBannerGif = extension == ".gif";

                var bannerFileName = $"banner_{Guid.NewGuid()}{extension}";
                var bannerFilePath = Path.Combine("wwwroot", "uploads", "banners", bannerFileName);
                var bannerDirectory = Path.GetDirectoryName(bannerFilePath);
                if (!string.IsNullOrEmpty(bannerDirectory)) Directory.CreateDirectory(bannerDirectory);

                using (var stream = new FileStream(bannerFilePath, FileMode.Create))
                {
                    BannerFile.CopyTo(stream);
                }

                bannerPath = $"/uploads/banners/{bannerFileName}";
            }
            var bioContent = string.IsNullOrWhiteSpace(model.CharacterBio) || model.CharacterBio.Length < 10 ? "<p><em>Under Construction</em></p>" : model.CharacterBio;

            string customUrl;

            if (!string.IsNullOrWhiteSpace(model.CustomUrl))
            {
                customUrl = Slugify(model.CustomUrl);

                // Check uniqueness
                bool exists = _context.Characters.Any(c => c.CustomUrl == customUrl);
                if (exists)
                {
                    ModelState.AddModelError("CustomUrl", "That custom URL is already taken.");
                    return View(model);
                }
            }
            else
            {
                var safeFirstName = model.FirstName?.Trim().ToLower().Replace(" ", "-") ?? "char";
                var suffix = Guid.NewGuid().ToString("N")[..6];
                customUrl = $"{safeFirstName}-{suffix}";
            }

            var isValid = Regex.IsMatch(customUrl, @"^[a-z0-9\-]+$");
            if (!isValid)
            {
                ModelState.AddModelError("CustomUrl", "Custom URL may only contain lowercase letters, numbers, and hyphens.");
                return View(model);
            }

            var character = new CharacterModel
            {
                UserAccountId = userId.Value,
                DisplayName = model.DisplayName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CharacterBio = bioContent,
                AvatarUrl = avatarPath ?? string.Empty,
                BannerUrl = bannerPath ?? string.Empty,
                BannerOffsetY = model.BannerOffsetY,
                IsBannerGif = isBannerGif,
                CharacterGender = model.CharacterGender,
                CharacterOrientation = model.CharacterOrientation,
                CustomUrl = customUrl,
                Pronouns = model.Pronouns,
                Species = model.Species,
                Verse = model.Verse,
                Occupation = model.Occupation,
                CreatedOn = DateTime.UtcNow,
                IsPublic = true,
                IsArchived = false
            };

            _context.Characters.Add(character);
            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }

        private string Slugify(string input)
        {
            return Regex.Replace(input.ToLower().Trim(), @"[^a-z0-9]+", "-").Trim('-');
        }
    }
}
