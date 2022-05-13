using Pastebook.Web.Models;
using System;

namespace Pastebook.Web.DataTransferObjects
{
    public class AlbumPhotoFormDTO
    {
        public Guid AlbumId { get; set; }

        public string AlbumName { get; set; }

        public FileUpload Photo { get; set; }
    }
}
