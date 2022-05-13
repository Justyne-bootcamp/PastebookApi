using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.Services;
using System;
using System.Threading.Tasks;
using Pastebook.Web.DataTransferObjects;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly INotificationService _notificationService;
        private readonly IPostService _postService;

        public CommentController(ICommentService CommentService, INotificationService notificationService, IPostService postService)
        {
            _commentService = CommentService;
            _notificationService = notificationService;
            _postService = postService;
        }

        [HttpPost]
        [Route("/addComment")]
        public async Task<IActionResult> AddComment([FromForm] CommentFormDTO commentForm)
        {
            var posterId = _postService.GetUserAccountIdByPost(commentForm.PostId);
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            Notification notification = new Notification()
            {
                NotificationId = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                NotificationStatus = "Unread",
                NotificationType = "Comment",
                NotificationPath = $@"http://site/posts/{commentForm.PostId}",
                UserAccountId = posterId,
                NotificationSourceId = userAccountId,
            };
            var comment = new Comment()
            {
                CommentId = Guid.NewGuid(),
                PostId = commentForm.PostId,
                CommentContent = commentForm.CommentContent,
                UserAccountId =  userAccountId
            };
            var add = await _commentService.Insert(comment);
            var notif = await _notificationService.Insert(notification);

            return StatusCode(StatusCodes.Status201Created, add);
        }

        [HttpDelete]
        [Route("/deleteComment/{id:Guid}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var delete = await _commentService.Delete(id);
            return StatusCode(StatusCodes.Status200OK, delete);
        }
    }
}
