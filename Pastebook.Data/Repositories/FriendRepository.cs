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
                    FriendRequestSender = f.FriendRequestSender,
                    FriendRequestStatus = f.FriendRequestStatus,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfilePhotoPath = u.ProfilePhotoPath,
                })
                .Where(e => e.FriendRequestSender.Equals(userAccountId) && e.FriendRequestStatus.Equals(status))
                .ToList();

            return friends;
        }
        public IEnumerable<FriendDTO> GetFriendRequests(Guid userAccountId, string status)
        {
            var friends = this.Context.Friends.Join(
                this.Context.UserAccounts,
                f => f.FriendRequestSender,
                u => u.UserAccountId,
                (f, u) => new FriendDTO
                {
                    FriendId = f.FriendId,
                    FriendRequestReceiver = f.FriendRequestReceiver,
                    FriendRequestSender = f.FriendRequestSender,
                    FriendRequestStatus = f.FriendRequestStatus,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfilePhotoPath = u.ProfilePhotoPath,
                })
                .Where(e => e.FriendRequestReceiver.Equals(userAccountId) && e.FriendRequestStatus.Equals(status))
                .ToList();

            return friends;
        }
    }
}
