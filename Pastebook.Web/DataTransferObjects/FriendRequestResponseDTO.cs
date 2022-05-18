using System;

namespace Pastebook.Web.DataTransferObjects
{
    public class FriendRequestResponseDTO
    {
        public Guid FriendId { get; set; }
        public string Response { get; set; }
    }

    public class AddFriendForm
    {
        public string UserAccountId { get; set; }
        public string FriendRequestReceiver { get; set; }
    }
}