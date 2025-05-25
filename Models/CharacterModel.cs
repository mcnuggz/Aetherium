using Aetherium.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aetherium.Models
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? BannerUrl { get; set; }
        public int? BannerOffsetY { get; set; }
        public bool IsBannerGif { get; set; }
        [MaxLength(10000)]
        public string? CharacterBio { get;set; }
        public GenderEnum CharacterGender { get; set; }
        public OrientationEnum CharacterOrientation { get; set; }
        public string? Pronouns { get; set; }
        public string? Species { get; set; }
        public string? Verse { get; set; }
        public string? Occupation { get; set; }
        public MoodEnum CharacterMood { get; set; }
        public string? CustomMood { get; set; }
        public string? CustomUrl { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsPublic { get; set; }
        public bool IsArchived { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        public UserModel UserAccount { get; set; } = null!;
        public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
        public ICollection<CharacterRelationshipModel> RelationshipsInitiated { get; set; } = new List<CharacterRelationshipModel>();
        public ICollection<CharacterRelationshipModel> RelationshipsReceived { get; set; } = new List<CharacterRelationshipModel>();
        public ICollection<AlbumModel> Albums { get; set; } = new List<AlbumModel>();
        public ICollection<PhotoModel> Photos { get; set; } = new List<PhotoModel>();

    }
}