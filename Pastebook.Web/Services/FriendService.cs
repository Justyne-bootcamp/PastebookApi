using Pastebook.Data.Models;
using Pastebook.Data.Models.DataTransferObjects;
using Pastebook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface IFriendService
    {
        public IEnumerable<FriendDTO> GetAllFriends(Guid userAccountId, string status);
        public IEnumerable<FriendDTO> GetFriendRequests(Guid userAccountId, string status);
        public Task<Friend> AddFriend(Friend friend);
        public Task<Friend> FindById(Guid friendId);
        public Task<Friend> Update(Friend friend);
    }
    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        public FriendService(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }
        public IEnumerable<FriendDTO> GetAllFriends(Guid userAccountId, string status)
        {
            var adddedFriends = _friendRepository.GetAddedFriends(userAccountId, status);
            var friendRequests = _friendRepository.GetFriendRequests(userAccountId, status);
            var friends = adddedFriends.Concat(friendRequests);
            return friends;
        }
        public IEnumerable<FriendDTO> GetFriendRequests(Guid userAccountId, string status)
        {
            var friendRequests = _friendRepository.GetFriendRequests(userAccountId, status);
            return friendRequests;
        }
        public async Task<Friend> AddFriend(Friend friend)
        {
            return await _friendRepository.Insert(friend);
        }

        public async Task<Friend> FindById(Guid friendId)
        {
            return await _friendRepository.FindByPrimaryKey(friendId);
        }

        public async Task<Friend> Update(Friend friend)
        {
            return await _friendRepository.Update(friend);
        }
    }
}
