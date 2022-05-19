using Pastebook.Data.Data;
using Pastebook.Data.Models;
using Pastebook.Data.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Repositories
{
    public interface IFriendRepository: IBaseRepository<Friend>
    {
        public IEnumerable<FriendDTO> GetAddedFriends(Guid userAccountId, string status);
        public IEnumerable<FriendDTO> GetFriendRequests(Guid userAccountId, string status);
        public IEnumerable<Guid> GetFriendsId(Guid userAccountId);

        public FriendRequestResponseDTO GetRelationship(Guid userAccountId, Guid receiverAccountId);
    }
    public class FriendRepository : GenericRepository<Friend>, IFriendRepository
    {
        public FriendRepository(PastebookContext context) : base(context)
        {
        }
        public IEnumerable<FriendDTO> GetAddedFriends(Guid userAccountId, string status)
        {
            var friends = this.Context.Friends.Join(
                this.Context.UserAccounts,
                f => f.FriendRequestReceiver,
                u => u.UserAccountId,
                (f, u) => new FriendDTO
                {
                    FriendId = f.FriendId,
                    FriendRequestReceiver = f.FriendRequestReceiver,
                    UserAccountId = f.UserAccountId,
                    FriendRequestStatus = f.FriendRequestStatus,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfilePhotoPath = u.ProfilePhotoPath,
                    Username = u.Username,
                })
                .Where(e => e.UserAccountId.Equals(userAccountId) && e.FriendRequestStatus.Equals(status))
                .ToList();

            return friends;
        }
        public IEnumerable<FriendDTO> GetFriendRequests(Guid userAccountId, string status)
        {
            var friends = this.Context.Friends.Join(
                this.Context.UserAccounts,
                f => f.UserAccountId,
                u => u.UserAccountId,
                (f, u) => new FriendDTO
                {
                    FriendId = f.FriendId,
                    FriendRequestReceiver = f.FriendRequestReceiver,
                    UserAccountId = f.UserAccountId,
                    FriendRequestStatus = f.FriendRequestStatus,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfilePhotoPath = u.ProfilePhotoPath,
                    Username = u.Username,
                })
                .Where(e => e.FriendRequestReceiver.Equals(userAccountId) && e.FriendRequestStatus.Equals(status))
                .ToList();

            return friends;
        }

        public IEnumerable<Guid> GetFriendsId(Guid userAccountId)
        {
            var addedFriends = this.Context.Friends.Select(e => e)
                .Where(e => e.UserAccountId.Equals(userAccountId) && e.FriendRequestStatus.Equals("Accepted"))
                .ToList();

            var friendsAddedYou = this.Context.Friends.Select(e => e)
                .Where(e => e.FriendRequestReceiver.Equals(userAccountId) && e.FriendRequestStatus.Equals("Accepted"))
                .ToList();
            var friendsIdList = new List<Guid>();

            foreach (var friend in addedFriends)
            {
                friendsIdList.Add(friend.FriendRequestReceiver);
            }

            foreach (var friend in friendsAddedYou)
            {
                friendsIdList.Add(friend.UserAccountId);
            }

            return friendsIdList;
        }

        public FriendRequestResponseDTO GetRelationship(Guid userAccountId, Guid receiverAccountId)
        {
            var sender = this.Context.Friends
                .Select(e => e)
                .Where(e => 
                (e.UserAccountId.Equals(userAccountId) && e.FriendRequestReceiver.Equals(receiverAccountId))
                )
                .FirstOrDefault();

            var receiver = this.Context.Friends
                .Select(e => e)
                .Where(e =>
                (e.FriendRequestReceiver.Equals(userAccountId) && e.UserAccountId.Equals(receiverAccountId))
                )
                .FirstOrDefault();
            if (sender is object)
            {
                var relationship = "";
                if (sender.FriendRequestStatus.Equals("Accepted"))
                {
                    relationship = "Accepted";
                }
                if (sender.FriendRequestStatus.Equals("Pending"))
                {
                    relationship = "pendingresponse";
                }
                return new FriendRequestResponseDTO()
                {
                    FriendId = sender.FriendId,
                    Response = relationship
                };
            }
            if (receiver is object)
            {
                var relationship = "";
                if (receiver.FriendRequestStatus.Equals("Accepted"))
                {
                    relationship = "Accepted";
                }
                if (receiver.FriendRequestStatus.Equals("Pending"))
                {
                    relationship = "pendingrequest";
                }
                return new FriendRequestResponseDTO()
                {
                    FriendId = receiver.FriendId,
                    Response = relationship,
                };
            }

            return new FriendRequestResponseDTO()
            {
                FriendId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Response = "stranger"
            };
        }
    }
}
