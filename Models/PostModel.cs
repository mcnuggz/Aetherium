using Aetherium.Enums;

namespace Aetherium.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedOn { get; set; }
        public PostPrivacyLevelEnum PrivacyLevel { get; set; }
        public RelationshipTypeEnum? AllowedRelationshipType { get; set; }
        public CharacterModel Character { get; set; } = null!;

        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public ICollection<ReactionModel> Reactions { get; set; } = new List<ReactionModel>();
    }
}