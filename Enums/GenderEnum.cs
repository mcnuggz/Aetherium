using System.ComponentModel.DataAnnotations;

namespace Aetherium.Enums
{
    public enum GenderEnum
    {
        // Will likely add more based on user feedback, I only know so much. I mean no disrespect. <3
        [Display(Name = "Unspecified")]
        Unspecified = 0,

        [Display(Name ="Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,

        [Display(Name = "Shemale/Futanari")]
        Futanari = 3,

        [Display(Name = "Transgender (MTF)")]
        TransgenderMTF = 4,

        [Display(Name = "Transgender (FTM)")]
        TransgenderFTM = 5,

        [Display(Name = "Non-Binary")]
        NonBinary = 6,

        [Display(Name = "Intersex")]
        Intersex = 7,

        [Display(Name = "Other / Custom")]
        Custom = 8

    }
}
