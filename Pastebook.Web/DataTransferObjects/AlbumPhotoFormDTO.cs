using Microsoft.AspNetCore.Http;
using System;

namespace Pastebook.Web.DataTransferObjects
{
    public class AlbumPhotoFormDTO
    {
        public string AlbumId { get; set; }

        public string AlbumName { get; set; }
        public string Username { get; set; }

        public IFormFile Photo { get; set; }
    }
}
