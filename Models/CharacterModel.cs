using System.ComponentModel.DataAnnotations.Schema;

namespace Aetherium.Models
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public string CharacterBio { get;set; }
        public DateTime? LastLoggedIn { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        public UserModel UserAccount { get; set; }
        public ICollection<PostModel> Posts { get; set; }
    }
}