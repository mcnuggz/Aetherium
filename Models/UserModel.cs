using Aetherium.Enums;
using Microsoft.AspNetCore.Identity;

namespace Aetherium.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRoleEnum Role { get; set; }
        public bool IsSuspended { get; set; }
        public int SuspensionDuration { get; set; }
        public DateTime? SuspensionExpirationDate { get; set; }
        public bool IsIPBanned { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LastKnownIP { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string EmailConfirmationToken { get; set; }
        public DateTime? TokenGeneratedAt { get; set; }
        public bool AgreedToCoC { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiration { get; set; }
        public ICollection<CharacterModel> Characters { get; set; }
    }
}
