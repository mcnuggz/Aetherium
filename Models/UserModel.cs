using Microsoft.AspNetCore.Identity;

namespace Aetherium.Models
{
    public class UserModel : IdentityUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public ICollection<CharacterModel> Characters { get; set; }
    }
}
