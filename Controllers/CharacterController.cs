using Aetherium.Data;
using Aetherium.Models;
using Aetherium.Models.ViewModels;
using Aetherium.Services;
using Microsoft.AspNetCore.Mvc;
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

            var character = new CharacterModel
            {
                UserAccountId = userId.Value,
                DisplayName = model.DisplayName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CharacterBio = model.CharacterBio,
                AvatarUrl = avatarPath ?? string.Empty,
                BannerUrl = bannerPath ?? string.Empty,
                BannerOffsetY = model.BannerOffsetY,
                IsBannerGif = isBannerGif,
                CharacterGender = model.CharacterGender,
                CharacterOrientation = model.CharacterOrientation,
                Pronouns = model.Pronouns,
                Species = model.Species,
                OriginWorld = model.OriginWorld,
                Occupation = model.Occupation,
                CreatedOn = DateTime.UtcNow,
                IsPublic = true,
                IsArchived = false
            };

            _context.Characters.Add(character);
            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
