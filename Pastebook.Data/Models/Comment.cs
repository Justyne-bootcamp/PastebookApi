using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Comment : BaseEntity
    {

        [Key]
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid? UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        public string CommentContent { get; set; }
    }
}
