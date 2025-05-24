using System.ComponentModel.DataAnnotations;

namespace Aetherium.Enums
{
    public enum UserRoleEnum
    {
        [Display(Name = "Admin")]
        Admin = 3,
        [Display(Name = "Mod")]
        Mod = 2,
        [Display(Name = "User")]
        User = 1,
    }
}
