using System;
using System.Collections.Generic;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Post : BaseEntity
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }

        public Guid PostId { get; set; }
        public Guid UserAccountId { get; set; }
        public string TextContent { get; set; }
        public string PostPhotoPath { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid PostLocation { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}
