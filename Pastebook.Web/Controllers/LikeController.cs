using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
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
        public async Task<IActionResult> LikePost(string postStringId)
        {
            var sessionId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var postId = Guid.Parse(postStringId);
            var likePost = _likeService.GetLikeByPostIdAndUserId(sessionId, postId);
            if (likePost == null)
            {
                var newLike = new Like()
                {
                    LikeId = Guid.NewGuid(),
                    PostId = postId,
                    UserAccountId = sessionId
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
                    NotificationSourceId = sessionId,
                };
                await _likeService.Insert(newLike);
                var notif = await _notificationService.Insert(notification);
                return StatusCode(StatusCodes.Status201Created, likePost);
            }
            if(likePost != null)
            {
                await _likeService.Delete(likePost.LikeId);
                return StatusCode(StatusCodes.Status202Accepted, likePost);
            }
            return StatusCode(StatusCodes.Status400BadRequest, likePost);

        }
    }
}
