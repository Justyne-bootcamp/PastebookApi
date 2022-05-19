using System;

namespace Pastebook.Web.DataTransferObjects
{
    public class AddFriendForm
    {
        public string UserAccountId { get; set; }
        public string FriendRequestReceiver { get; set; }
    }

    public class RespondToFriendRequest
    {
        public string FriendId { get; set; }
        public string Response { get; set; }
    }
}