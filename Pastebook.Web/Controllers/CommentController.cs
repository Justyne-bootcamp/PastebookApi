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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly INotificationService _notificationService;

        public CommentController(ICommentService CommentService, INotificationService notificationService)
        {
            _commentService = CommentService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var comment = await _commentService.FindAll();
            return StatusCode(StatusCodes.Status200OK, comment);
        }

        [HttpPost]
        [Route("/addComment/{comment}")]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            Notification notification = new Notification()
            {
                NotificationId = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                NotificationStatus = "Unread",
                NotificationType = "Post",
                NotificationPath = $@"http://site/posts/{comment.PostId}",
                UserAccountId = Guid.NewGuid(),//dummy account Id for now. must get this from UI together with the postId
                NotificationSourceId = (Guid)comment.UserAccountId,
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
