using Aetherium.Enums;

namespace Aetherium.Models.ViewModels
{
    public class PostViewModel
    {
        public string PostContent { get; set; }
        public PostPrivacyLevelEnum PrivacyLevel { get; set; }
        public RelationshipTypeEnum? AllowedRelationshipType { get; set; }

        public int PostId { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CharacterId { get; set; }
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public MoodEnum CharacterMood { get; set; }
        public string? CustomMood { get; set; }

        public int CommentCount { get; set; }
        public List<CommentModel> Comments { get; set; }
        public Dictionary<string, int> Reactions { get; set; } = new Dictionary<string, int>();

    }
}
