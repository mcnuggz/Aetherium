using Aetherium.Enums;
using Microsoft.AspNetCore.Identity;

namespace Aetherium.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public UserRoleEnum Role { get; set; }
        public bool IsSuspended { get; set; }
        public int SuspensionDuration { get; set; }
        public DateTime? SuspensionExpirationDate { get; set; }
        public bool IsIPBanned { get; set; }
        public ICollection<CharacterModel> Characters { get; set; }
    }
}
