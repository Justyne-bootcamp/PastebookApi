using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Models.DataTransferObjects
{
    public class CommentDTO
    {
        public Guid CommentId { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid PostId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CommentContent { get; set; }
    }
}
