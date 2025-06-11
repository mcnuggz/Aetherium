using Aetherium.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Aetherium.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<CharacterModel> Characters { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<AlbumModel> Albums { get; set; }
        public DbSet<PhotoModel> Photos { get; set; }
        public DbSet<CharacterRelationshipModel> CharacterRelationships { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<ReactionModel> Reactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CharacterModel>()
                .HasOne(c => c.UserAccount)
                .WithMany(u => u.Characters)
                .HasForeignKey(c => c.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CharacterRelationshipModel>()
                .HasOne(r => r.CharacterA)
                .WithMany(c => c.RelationshipsInitiated)
                .HasForeignKey(r => r.CharacterAId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharacterRelationshipModel>()
                .HasOne(r => r.CharacterB)
                .WithMany(c => c.RelationshipsReceived)
                .HasForeignKey(r => r.CharacterBId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AlbumModel>()
                .HasOne(a => a.Character)
                .WithMany(c => c.Albums)
                .HasForeignKey(a => a.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PhotoModel>()
                .HasOne(p => p.Album)
                .WithMany(a => a.Photos)
                .HasForeignKey(p => p.AlbumId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<PhotoModel>()
                .HasOne(p => p.Character)
                .WithMany(c => c.Photos)
                .HasForeignKey(p => p.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharacterModel>()
                .HasIndex(c => c.CustomUrl)
                .IsUnique();

            builder.Entity<PostModel>()
                .HasOne(p => p.Character)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
