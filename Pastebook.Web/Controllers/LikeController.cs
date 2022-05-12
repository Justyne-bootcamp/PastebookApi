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
        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost, Route("{postId:Guid}")]
        public async Task<IActionResult> LikePost([FromRoute] Guid postId)
        {
            var sessionId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var likePost = _likeService.GetLikeByPostIdAndUserId(sessionId, postId);
            if (likePost == null)
            {
                var newLike = new Like()
                {
                    LikeId = Guid.NewGuid(),
                    PostId = postId,
                    UserAccountId = sessionId
                };
                await _likeService.Insert(newLike);
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
