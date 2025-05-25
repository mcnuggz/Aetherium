using Aetherium.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aetherium.Models.ViewModels
{
    public class CharacterCreateViewModel
    {
        [Display(Name ="Display Name")]
        public string? DisplayName { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string? LastName { get; set; }
        [Display(Name = "Character Bio")]
        [MaxLength(10000)]
        public string? CharacterBio { get; set; }
        public string? CroppedAvatarBase64 { get; set; }
        [Display(Name = "Custom URL")]
        public string? CustomUrl { get; set; }
        public int? BannerOffsetY { get; set; }
        public bool IsBannerGif { get; set; }
        [Display(Name = "Gender")]
        public GenderEnum CharacterGender { get; set; }
        [Display(Name = "Sexual Orientation")]
        public OrientationEnum CharacterOrientation { get; set; }
        [Display(Name = "Pronouns")]
        public string? Pronouns { get; set; }
        [Display(Name = "Species")]
        public string? Species { get; set; }
        [Display(Name = "Verse")]
        public string? Verse { get; set; }
        [Display(Name = "Occupation")]
        public string? Occupation { get; set; }
    }
}
