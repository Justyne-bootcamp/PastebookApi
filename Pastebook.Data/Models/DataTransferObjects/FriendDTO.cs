using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Models.DataTransferObjects
{
    public class FriendDTO
    {
        public Guid FriendId { get; set; }
        public Guid FriendRequestSender { get; set; }
        public Guid FriendRequestReceiver { get; set; }
        public string FriendRequestStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePhotoPath { get; set; }
    }
}
