using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Friend : BaseEntity
    {
        public Guid FriendId { get; set; }
        public Guid FriendRequestSender { get; set; }
        public Guid FriendRequestReceiver { get; set; }

        [Required]
        public string FriendRequestStatus { get; set; }

    }
}
