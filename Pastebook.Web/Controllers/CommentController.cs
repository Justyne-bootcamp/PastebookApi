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
        private readonly IUserAccountService _userAccountService;

        public CommentController(ICommentService CommentService, IUserAccountService userAccountService)
        {
            _commentService = CommentService;
            _userAccountService = userAccountService;
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
            var add = await _commentService.Insert(comment);
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
