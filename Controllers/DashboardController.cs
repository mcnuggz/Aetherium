using Aetherium.Data;
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
                .OrderByDescending(c => c.CreatedOn)
                .ToList();

            if (!userCharacters.Any()) { return RedirectToAction("Create", "Character"); }
            return View(userCharacters);
        }
    }
}
