using System.ComponentModel.DataAnnotations;

namespace Aetherium.Enums
{
    public enum PostPrivacyLevelEnum
    {
        [Display(Name = "Public")]
        Public = 0,

        [Display(Name = "Friends Only")]
        Private = 1,

        [Display(Name = "Only Me")]
        Personal = 2,

        [Display(Name = "Specific Relationship")]
        SpecificRelationship = 3,
    }
}
