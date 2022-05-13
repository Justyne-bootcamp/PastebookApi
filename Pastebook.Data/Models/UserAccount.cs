using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class UserAccount : BaseEntity
    {
        public UserAccount()
        {
            Albums = new HashSet<Album>();
            Comments = new HashSet<Comment>();
            Friends = new HashSet<Friend>();
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
        }

        [Key]
        public Guid UserAccountId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public char Gender { get; set; }
        public string MobileNumber { get; set; }
        public string AboutMe { get; set; }
        public string ProfilePhotoPath { get; set; }

        [Required]
        public string Username { get; set; }

        public ICollection<Friend> Friends { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
