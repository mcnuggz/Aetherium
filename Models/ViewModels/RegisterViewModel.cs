using System.ComponentModel.DataAnnotations;

namespace Aetherium.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
