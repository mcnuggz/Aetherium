using Aetherium.Models;
using Microsoft.EntityFrameworkCore;

namespace Aetherium.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CharacterModel> Characters { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
