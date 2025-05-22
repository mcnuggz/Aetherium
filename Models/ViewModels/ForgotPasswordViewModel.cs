using System.ComponentModel.DataAnnotations;

namespace Aetherium.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
