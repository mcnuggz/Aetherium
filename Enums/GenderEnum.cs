using System.ComponentModel.DataAnnotations;

namespace Aetherium.Enums
{
    public enum GenderEnum
    {
        // Will likely add more based on user feedback, I only know so much. I mean no disrespect. <3
        [Display(Name ="Male")]
        Male,
        [Display(Name = "Female")]
        Female,
        [Display(Name = "Shemale/Futanari")]
        Futanari,
        [Display(Name = "Transexual")]
        Transexual,
        [Display(Name = "Intersex")]
        Intersex,
        [Display(Name = "Other")]
        Other
    }
}
