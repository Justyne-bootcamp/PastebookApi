using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Friend : BaseEntity
    {
        [Key]
        public Guid FriendId { get; set; }

        [Required]
        public string FriendRequestStatus { get; set; }
        [Required]
        public Guid FriendRequestReceiver { get; set; }

        //user account id
        public Guid UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

    }
}
