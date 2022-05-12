using System;
using Pastebook.Web.Models;

namespace Pastebook.Web.DataTransferObjects
{
    public class PostFormDTO
    {
        public string TextContent { get; set; }
        public Guid PostLocation { get; set; }
        public FileUpload Photo { get; set; }
    }
}
