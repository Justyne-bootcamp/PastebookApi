using System;
using System.Collections.Generic;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class Album : BaseEntity
    {
        public Album()
        {
            AlbumPhotos = new HashSet<AlbumPhoto>();
        }

        public Guid AlbumId { get; set; }
        public string AlbumName { get; set; }
        public Guid UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<AlbumPhoto> AlbumPhotos { get; set; }
    }
}
