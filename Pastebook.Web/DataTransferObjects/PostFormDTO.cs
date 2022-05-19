using System;
using Microsoft.AspNetCore.Http;
using Pastebook.Web.Models;

namespace Pastebook.Web.DataTransferObjects
{
    public class PostFormDTO
    {
        public string TextContent { get; set; }
        public Guid PostLocation { get; set; }
        public Guid SessionId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
