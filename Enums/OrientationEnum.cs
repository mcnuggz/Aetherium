using System.ComponentModel.DataAnnotations;

namespace Aetherium.Enums
{
    public enum OrientationEnum
    {
        // Will likely add more based on user feedback, I only know so much. I mean no disrespect. <3
        [Display(Name ="Heterosexual")]
        Heterosexual,
        [Display(Name = "Gay")]
        Gay,
        [Display(Name = "Lesbian")]
        Lesbian,
        [Display(Name = "Bi - Female Lean")]
        BiF,
        [Display(Name = "Bi - Male Lean")]
        BiM,
        [Display(Name = "Asexual")]
        Asexual,
        [Display(Name = "Unsure/Confused")]
        Confused

    }
}
