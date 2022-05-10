using System;
using System.Collections.Generic;

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

        public Guid UserAccountId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public int? MobileNumber { get; set; }
        public string AboutMe { get; set; }
        public string ProfilePhotoPath { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
