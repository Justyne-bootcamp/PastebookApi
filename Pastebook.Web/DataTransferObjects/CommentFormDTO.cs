using System;

namespace Pastebook.Web.DataTransferObjects
{
    public class CommentFormDTO
    {
        public Guid PostId { get; set; }
        public string CommentContent { get; set; }
    }
}
