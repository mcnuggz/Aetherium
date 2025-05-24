using Aetherium.Models;
using Microsoft.AspNetCore.Identity;

namespace Aetherium.Services
{
    public class PasswordService
    {
        public string Hash(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        public bool Verify(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }

        public void SetNewPassword(UserModel user, string newPassword) {
            var hasher = new PasswordHasher<UserModel>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);
        }
    }
}
