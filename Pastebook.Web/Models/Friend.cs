using System;
using System.Collections.Generic;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Friend : BaseEntity
    {
        public Guid FriendId { get; set; }
        public Guid OwnAccountId { get; set; }
        public Guid UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
