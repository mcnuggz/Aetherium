using Aetherium.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aetherium.Models.ViewModels
{
    public class CharacterCreateViewModel
    {
        [Required]
        public string DisplayName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CharacterBio { get; set; }
        public string CroppedAvatarBase64 { get; set; }

        public string BannerUrl { get; set; }
        public int? BannerOffsetY { get; set; }
        public bool IsBannerGif { get; set; }

        public GenderEnum CharacterGender { get; set; }
        public OrientationEnum CharacterOrientation { get; set; }
        public string Pronouns { get; set; }

        public string Species { get; set; }
        public string OriginWorld { get; set; }
        public string Occupation { get; set; }
    }
}
