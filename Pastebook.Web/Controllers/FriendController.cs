using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.Services;
using System;
using Pastebook.Web.Http;
using Pastebook.Data.Models.DataTransferObjects;
using System.Threading.Tasks;
using Pastebook.Web.DataTransferObjects;

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
                UserAccountId = Guid.Parse(addFriendForm.UserAccountId),
                FriendRequestReceiver = Guid.Parse(addFriendForm.FriendRequestReceiver),
                FriendRequestStatus = "Pending"
            };
            var addedFriend = await _friendService.AddFriend(friend);
            Notification notification = new Notification()
            {
                NotificationId = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                NotificationStatus = "Unread",
                NotificationType = "FriendRequest",
                NotificationPath = "http://site/friends",
                UserAccountId = Guid.Parse(addFriendForm.FriendRequestReceiver),
                NotificationSourceId = Guid.Parse(addFriendForm.UserAccountId),
            };


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

        [HttpPost]
        [Route("relationship")]
        public IActionResult GetRelationship([FromBody] RelationshipForm relationshipForm)
        {
            var relationship = _friendService.GetRelationship(
                Guid.Parse(relationshipForm.UserAccountId),
                Guid.Parse(relationshipForm.ReceiverAccountId));
            return StatusCode(StatusCodes.Status200OK, new RelationshipResponse()
            {
                FriendId = relationship.FriendId.ToString(),
                Relationship = relationship.Response,
            });
        }

        [HttpPost]
        [Route("respondToRequest")]
        public async Task<IActionResult> RespondToRequest([FromBody] RespondToFriendRequest response) 
        {
            var existingFriend = await _friendService.FindById(Guid.Parse(response.FriendId));
            existingFriend.FriendRequestStatus = response.Response;
            var friend = _friendService.Update(existingFriend);

            return StatusCode(StatusCodes.Status200OK, friend);
        }

    }
}
