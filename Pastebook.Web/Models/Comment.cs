using System;
using System.Collections.Generic;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Comment : BaseEntity
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public string CommentContent { get; set; }
        public Guid? UserAccountId { get; set; }

        public virtual Post Post { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
