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
        public virtual DbSet<Notification> Notifications { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=DMMBQG3;Database=PastebookDb;User Id=sa;Password=lastgam3counts");
        //    }
        //}
    }
}
