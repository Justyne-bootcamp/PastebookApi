using System;
using System.Collections.Generic;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class AlbumPhoto : BaseEntity
    {
        public Guid AlbumPhotoId { get; set; }
        public Guid AlbumId { get; set; }
        public string AlbumPhotoPath { get; set; }

        public virtual Album Album { get; set; }
    }
}
