using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Like : BaseEntity
    {
        [Key]
        public Guid LikeId { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        //userId of liker
        public Guid? UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
