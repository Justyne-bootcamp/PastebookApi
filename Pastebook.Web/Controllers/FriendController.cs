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
        [Route("addFriend")]
        public async Task<IActionResult> AddFriend([FromBody] AddFriendForm addFriendForm)
        {
            Friend friend = new Friend()
            {
                FriendId = Guid.NewGuid(),
                UserAccountId = Guid.Parse("7e501cc3-4f84-4633-9bac-feb5ed7f7692"),
                FriendRequestReceiver = Guid.Parse("9db09605-8a13-4755-99e9-9ccfe7356c3b"),
                FriendRequestStatus = "Pending"
            };
            var addedFriend = _friendService.AddFriend(friend);
            Notification notification = new Notification()
            {
                NotificationId = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                NotificationStatus = "Unread",
                NotificationType = "FriendRequest",
                NotificationPath = "http://site/friends",
                UserAccountId = Guid.Parse("9db09605-8a13-4755-99e9-9ccfe7356c3b"),
                NotificationSourceId = Guid.Parse("7e501cc3-4f84-4633-9bac-feb5ed7f7692"),
            };

            
            //var notif = await _notificationService.Insert(notification);
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
        [Route("respondToRequest/{friendId:Guid}")]
        public async Task<IActionResult> RespondToRequest([FromRoute] Guid friendId) 
        {
            FriendRequestResponseDTO response = new FriendRequestResponseDTO()
            {
                FriendId = friendId,
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
