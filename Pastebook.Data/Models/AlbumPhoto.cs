using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pastebook.Data.Models
{
    public partial class AlbumPhoto : BaseEntity
    {
        [Key]
        public Guid AlbumPhotoId { get; set; }
        [Required]
        public string AlbumPhotoPath { get; set; }
        //FK
        public Guid AlbumId { get; set; }
        public  Album Album { get; set; }
    }
}
