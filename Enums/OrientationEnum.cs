using System.ComponentModel.DataAnnotations;

namespace Aetherium.Enums
{
    public enum OrientationEnum
    {
        // Will likely add more based on user feedback, I only know so much. I mean no disrespect. <3
        [Display(Name = "Unspecified")]
        Unspecified = 0,
        [Display(Name ="Heterosexual")]
        Heterosexual = 1,
        [Display(Name = "Gay")]
        Gay = 2,
        [Display(Name = "Lesbian")]
        Lesbian = 3,
        [Display(Name = "Bi - Female Lean")]
        BiF = 4,
        [Display(Name = "Bi - Male Lean")]
        BiM = 5,
        [Display(Name = "Asexual")]
        Asexual = 6,
        [Display(Name = "Unsure/Confused")]
        Confused = 7
    }
}
