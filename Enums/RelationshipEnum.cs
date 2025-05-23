using System.ComponentModel.DataAnnotations;

namespace Aetherium.Enums
{
    public enum RelationshipTypeEnum
    {
        [Display(Name = "Unspecified")]
        Unspecified = 0,

        [Display(Name = "Enemy")]
        Enemy = 1,

        [Display(Name = "Rival")]
        Rival = 2,

        [Display(Name = "Mentor")]
        Mentor = 3,

        [Display(Name = "Student")]
        Student = 4,

        [Display(Name = "Sister")]
        Sister = 5,

        [Display(Name = "Brother")]
        Brother = 6,

        [Display(Name = "Mother")]
        Mother = 7,

        [Display(Name = "Father")]
        Father = 8,

        [Display(Name = "Son")]
        Son = 9,

        [Display(Name = "Daughter")]
        Daughter = 10,

        [Display(Name = "Romantic Partner")]
        RomanticPartner = 11,

        [Display(Name = "Spouse")]
        Spouse = 12,

        [Display(Name = "Crush")]
        Crush = 13,

        [Display(Name = "Complicated")]
        Complicated = 14,

        [Display(Name = "Companion")]
        Companion = 15,

        [Display(Name = "Servant")]
        Servant = 16,

        [Display(Name = "Master")]
        Master = 17
    }
}
