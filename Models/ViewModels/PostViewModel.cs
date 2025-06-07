using Aetherium.Enums;

namespace Aetherium.Models.ViewModels
{
    public class PostViewModel
    {
        public string PostContent { get; set; }
        public PostPrivacyLevelEnum PrivacyLevel { get; set; }
        public RelationshipTypeEnum? AllowedRelationshipType { get; set; }
    }
}
