using Microsoft.AspNetCore.Http;

namespace Pastebook.Web.Models
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
