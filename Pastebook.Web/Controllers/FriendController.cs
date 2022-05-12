using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.Services;
using System;
using Pastebook.Web.Http;
using Pastebook.Data.Models.DataTransferObjects;
using Pastebook.Web.DataTransferObjects;
using System.Threading.Tasks;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : Controller
    {
        private readonly IFriendService _friendService;
        private readonly INotificationService _notificationService;

        public FriendController(IFriendService friendService, INotificationService notificationService)
        {
            _friendService = friendService;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("friends")]
        public IActionResult GetFriends()
        {            
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var friends = _friendService.GetAllFriends(userAccountId, "Accepted");
            if(friends != null)
            {
                return StatusCode(
                    StatusCodes.Status200OK, friends);
            }
            return StatusCode(StatusCodes.Status200OK, new FriendDTO());
        }

        [HttpPost]
        [Route("friendRequests")]
        public IActionResult GetFriendRequests()
        {
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var friends = _friendService.GetFriendRequests(userAccountId, "Pending");
            if (friends != null)
            {
                return StatusCode(
                    StatusCodes.Status200OK, friends);
            }
            return StatusCode(StatusCodes.Status200OK, new FriendDTO());
        }
        [HttpPost]
        [Route("addFriend/{id:Guid}")]
        public async Task<IActionResult> AddFriend(Guid id)
        {
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            Friend friend = new Friend()
            {
                FriendId = Guid.NewGuid(),
                FriendRequestSender = userAccountId,
                FriendRequestReceiver = id,
                FriendRequestStatus = "Pending"
            };

            Notification notification = new Notification()
            {
                NotificationId = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                NotificationStatus = "Unread",
                NotificationType = "FriendRequest",
                NotificationPath = "http://site/friends",
                UserAccountId = id,
                NotificationSourceId = userAccountId,
            };

            var addedFriend = _friendService.AddFriend(friend);
            var notif = await _notificationService.Insert(notification);
            if (addedFriend != null)
            {
                return StatusCode(
                    StatusCodes.Status201Created, addedFriend);
            }
            return StatusCode(
                StatusCodes.Status404NotFound, 
                new HttpResponseError()
                {
                    Message = "Failure to add a friend",
                    StatusCode = StatusCodes.Status404NotFound
                });
        }

        [HttpPut]
        [Route("respondToRequest")]
        public async Task<IActionResult> RespondToRequest()
        {
            FriendRequestResponseDTO response = new FriendRequestResponseDTO()
            {
                FriendId = Guid.Parse("4724E6EA-D440-4DD6-95D6-E1CCD219797D"),
                Response = "Accepted"
            };
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var existingFriend = await _friendService.FindById(response.FriendId);
            existingFriend.FriendRequestStatus = response.Response;
            var friend = _friendService.Update(existingFriend);

            return StatusCode(StatusCodes.Status200OK, friend);
        }

    }
}
