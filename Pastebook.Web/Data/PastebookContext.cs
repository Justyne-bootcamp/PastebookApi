using Microsoft.EntityFrameworkCore;
using Pastebook.Data.Models;

namespace Pastebook.Web.Data
{
    public partial class PastebookContext : DbContext
    {
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<AlbumPhoto> AlbumPhotos { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }

        public PastebookContext(DbContextOptions<PastebookContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasIndex(e => e.UserAccountId, "IX_Albums_UserAccountId");

                entity.Property(e => e.AlbumId).ValueGeneratedNever();

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.UserAccountId);
            });

            modelBuilder.Entity<AlbumPhoto>(entity =>
            {
                entity.HasIndex(e => e.AlbumId, "IX_AlbumPhotos_AlbumId");

                entity.Property(e => e.AlbumPhotoId).ValueGeneratedNever();

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.AlbumPhotos)
                    .HasForeignKey(d => d.AlbumId);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(e => e.PostId, "IX_Comments_PostId");

                entity.HasIndex(e => e.UserAccountId, "IX_Comments_UserAccountId");

                entity.Property(e => e.CommentId).ValueGeneratedNever();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId);

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserAccountId);
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasIndex(e => e.UserAccountId, "IX_Friends_UserAccountId");

                entity.Property(e => e.FriendId).ValueGeneratedNever();

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.UserAccountId);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasIndex(e => e.PostId, "IX_Likes_PostId");

                entity.HasIndex(e => e.UserAccountId, "IX_Likes_UserAccountId");

                entity.Property(e => e.LikeId).ValueGeneratedNever();

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.PostId);

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserAccountId);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(e => e.UserAccountId, "IX_Posts_UserAccountId");

                entity.Property(e => e.PostId).ValueGeneratedNever();

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserAccountId);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.Property(e => e.UserAccountId).ValueGeneratedNever();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Password).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
