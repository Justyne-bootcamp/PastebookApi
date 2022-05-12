using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static Pastebook.Web.Services.AlbumPhotoService;

namespace Pastebook.Web.Models
{
    public class FileUpload
    {
        [Required]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] {".jpg", ".jpeg", ".png"})]

        public IFormFile files { get; set; }
    }
}
