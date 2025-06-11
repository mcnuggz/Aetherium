using Aetherium.Data;
using Aetherium.Models;
using Aetherium.Models.ViewModels;
using Aetherium.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aetherium.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly UploadService _uploadService;

        public DashboardController(ApplicationDbContext context, UserService userService, UploadService uploadService)
        {
            _context = context;
            _userService = userService;
            _uploadService = uploadService;
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

            var currentCharacter = userCharacters.First();

            var posts = _context.Posts
                .Include(p => p.Character)
                .Where(p => p.Character.IsPublic || p.Character.UserAccountId == userId)
                .OrderByDescending(p => p.CreatedOn)
                .Select(post => new PostViewModel
                {
                    PostId = post.Id,
                    PostContent = post.PostContent,
                    CreatedOn = post.CreatedOn,
                    PrivacyLevel = post.PrivacyLevel,
                    AllowedRelationshipType = post.AllowedRelationshipType,

                    CharacterId = post.CharacterId,
                    DisplayName = post.Character.DisplayName ?? $"{post.Character.FirstName} {post.Character.LastName}",
                    AvatarUrl = post.Character.AvatarUrl ?? "/images/default-avatar.png",
                    CharacterMood = post.Character.CharacterMood,
                    CustomMood = post.Character.CustomMood,

                    Comments = _context.Comments
                        .Where(c => c.PostId == post.Id)
                        .Include(c => c.Character)
                        .OrderBy(c => c.CreatedOn)
                        .ToList(),

                    CommentCount = _context.Comments.Count(c => c.PostId == post.Id),

                    // Reactions = _reactionService.GetReactionCounts(post.Id) // Optional
                }).ToList();

            var viewModel = new DashboardViewModel
            {
                CurrentCharacter = currentCharacter,
                AllCharacters = userCharacters,
                Posts = posts,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStatus(PostViewModel model)
        {

            // we want to add the upload service call somewhere in here
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

        [HttpPost("/api/upload/image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { error = "No image provided." });

            try
            {
                var imageUrl = await _uploadService.UploadImageAsync(image, "status");
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(int postId, string content)
        {
            var userId = _userService.GetUserId();
            var character = _context.Characters.FirstOrDefault(c => c.UserAccountId == userId && !c.IsArchived);
            if(character == null || string.IsNullOrWhiteSpace(content))
            {
                TempData["StatusError"] = "Comment cannot be empty";
                return RedirectToAction("Index");
            }

            var comment = new CommentModel
            {
                PostId = postId,
                CharacterId = character.Id,
                Content = content,
                CreatedOn = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
