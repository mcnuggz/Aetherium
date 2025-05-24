using Aetherium.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aetherium.Models.ViewModels
{
    public class CharacterCreateViewModel
    {
        public string? DisplayName { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [MaxLength(10000)]
        public string CharacterBio { get; set; }
        public string? CroppedAvatarBase64 { get; set; }
        public string? CustomUrl { get; set; }
        public int? BannerOffsetY { get; set; }
        public bool IsBannerGif { get; set; }
        public GenderEnum CharacterGender { get; set; }
        public OrientationEnum CharacterOrientation { get; set; }
        public string? Pronouns { get; set; }
        public string? Species { get; set; }
        public string? Verse { get; set; }
        public string? Occupation { get; set; }
    }
}
