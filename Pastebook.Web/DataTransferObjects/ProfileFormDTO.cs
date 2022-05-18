using Microsoft.AspNetCore.Http;

namespace Pastebook.Web.DataTransferObjects
{
    public class ProfileFormDTO
    {
        public string UserAccountId { get; set; }
        public string AboutMe { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
