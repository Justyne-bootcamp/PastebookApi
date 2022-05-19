using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Models.DataTransferObjects
{
    public class PostDTO
    {
        public Guid PostId { get; set; }
        public Guid UserAccountId { get; set; }
        public string TextContent { get; set; }
        public string PostPhotoPath { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid PostLocation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string ProfilePhotoPath { get; set; }
        public bool isLiked { get; set; }

    }
}
