using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var like = _likeService.IsPostLiked(sessionId, postId);
            if(like == null)
            {
                like.UserAccountId = sessionId;
                like.PostId = postId;
                await _likeService.Insert(like);
                return StatusCode(StatusCodes.Status201Created, like);
            }
            if(like != null)
            {
                await _likeService.Delete(like.LikeId);
                return StatusCode(StatusCodes.Status202Accepted, like);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
