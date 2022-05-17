using Microsoft.AspNetCore.Http;

namespace Pastebook.Web.DataTransferObjects
{
    public class ProfileFormDTO
    {
        public string AboutMe { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
