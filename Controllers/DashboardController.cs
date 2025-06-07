using Aetherium.Data;
using Aetherium.Models;
using Aetherium.Models.ViewModels;
using Aetherium.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aetherium.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public DashboardController(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var userId = _userService.GetUserId();
            if (userId == null) return RedirectToAction("Index", "Home");

            var userCharacters = _context.Characters
                .Where(c => c.UserAccountId == userId && !c.IsArchived)
                .OrderByDescending(c => c.LastLoggedIn)
                .ToList();

            if (!userCharacters.Any()) { return RedirectToAction("Create", "Character"); }

            var viewModel = new DashboardViewModel
            {
                CurrentCharacter = userCharacters.First(),
                AllCharacters = userCharacters
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStatus(PostViewModel model)
        {
            var userId = _userService.GetUserId();

            var character = _context.Characters.FirstOrDefault(c => c.UserAccountId == userId && !c.IsArchived);

            if (string.IsNullOrWhiteSpace(model.PostContent))
            {
                TempData["StatusError"] = "Post content cannot be empty.";
                return RedirectToAction("Index");
            }

            if (character != null)
            {

                var post = new PostModel
                {
                    CharacterId = character.Id,
                    PostContent = model.PostContent,
                    CreatedOn = DateTime.UtcNow,
                    PrivacyLevel = model.PrivacyLevel,
                    AllowedRelationshipType = model.AllowedRelationshipType
                };

                _context.Posts.Add(post);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
