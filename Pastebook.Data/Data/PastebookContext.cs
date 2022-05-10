using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pastebook.Data.Models;

#nullable disable

namespace Pastebook.Data.Data
{
    public partial class PastebookContext : DbContext
    {
        public PastebookContext()
        {
        }

        public PastebookContext(DbContextOptions<PastebookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<AlbumPhoto> AlbumPhotos { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=FMMBQG3\\SQLEXPRESS;Database=PastebookDb;User Id=sa;Password=mikkodacasin03");
//            }
//        }

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
