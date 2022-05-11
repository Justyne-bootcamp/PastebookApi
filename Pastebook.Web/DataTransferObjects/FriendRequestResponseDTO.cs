using System;

namespace Pastebook.Web.DataTransferObjects
{
    public class FriendRequestResponseDTO
    {
        public Guid FriendId { get; set; }
        public string Response { get; set; }
    }
}
