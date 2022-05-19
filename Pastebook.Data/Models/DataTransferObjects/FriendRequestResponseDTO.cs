using System;

namespace Pastebook.Data.Models.DataTransferObjects
{
    public class FriendRequestResponseDTO
    {
        public Guid FriendId { get; set; }
        public string Response { get; set; }
    }
}