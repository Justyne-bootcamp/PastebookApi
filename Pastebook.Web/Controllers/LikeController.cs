using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Data.Models.DataTransferObjects;
using Pastebook.Web.Services;
using System;
using System.Threading.Tasks;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;
        private readonly IPostService _postService;
        private readonly INotificationService _notificationService;
        public LikeController(ILikeService likeService, IPostService postService, INotificationService notificationService)
        {
            _likeService = likeService;
            _postService = postService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLikersOfPost(string postStringId)
        {
            var postId = Guid.Parse(postStringId);
            var likers = _likeService.GetAllLikesByPostId(postId);
            return StatusCode(StatusCodes.Status200OK, likers);
        }

        [HttpPost]
        public async Task<IActionResult> LikePost([FromForm] LikeDTO likeDto)
        {
            //var sessionId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var userAccountId = Guid.Parse(likeDto.SessionId);
            var postId = Guid.Parse(likeDto.PostId);
            var likePost = _likeService.GetLikeByPostIdAndUserId(userAccountId, postId);
            if (likePost == null)
            {
                var newLike = new Like()
                {
                    LikeId = Guid.NewGuid(),
                    PostId = postId,
                    UserAccountId = userAccountId
                };
                var posterId = _postService.GetUserAccountIdByPost(postId);
                Notification notification = new Notification()
                {
                    NotificationId = Guid.NewGuid(),
                    TimeStamp = DateTime.Now,
                    NotificationStatus = "Unread",
                    NotificationType = "Like",
                    NotificationPath = $@"http://site/posts/{postId}",
                    UserAccountId = posterId,
                    NotificationSourceId = userAccountId,
                };
                await _likeService.Insert(newLike);
                var notif = await _notificationService.Insert(notification);
                return StatusCode(StatusCodes.Status201Created, newLike);
            }
            if(likePost != null)
            {
                await _likeService.Delete(likePost.LikeId);
                return StatusCode(StatusCodes.Status202Accepted, likePost);
            }
            return StatusCode(StatusCodes.Status400BadRequest, likePost);

        }

        [HttpGet]
        [Route("ifLiked")]
        public async Task<IActionResult> GetIfLiked([FromBody] string postStringId)
        {
            //var sessionId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var sessionId = Guid.Parse("610D9455-4E7B-4B78-A4CF-99E47A48FCBE");
            var postId = Guid.Parse(postStringId);
            var likedPost = _likeService.GetLikeByPostIdAndUserId(sessionId, postId);

            return StatusCode(StatusCodes.Status201Created, likedPost);
        }
    }
}
