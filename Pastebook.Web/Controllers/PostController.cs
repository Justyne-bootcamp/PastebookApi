using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.DataTransferObjects;
using Pastebook.Web.Http;
using Pastebook.Web.Models;
using Pastebook.Web.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserAccountService _userAccountService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFriendService _friendService;
        private readonly ICommentService _commentService;

        public PostController(IPostService PostService, IUserAccountService userAccountService, IWebHostEnvironment webHostEnvironment, IFriendService friendService, ICommentService commentService)
        {
            _postService = PostService;
            _userAccountService = userAccountService;
            _webHostEnvironment = webHostEnvironment;
            _friendService = friendService;
            _commentService = commentService;
        }

        [HttpPost]
        [Route("addPost")]
        public async Task<IActionResult> AddPost([FromForm] PostFormDTO postForm)
        {
            //var userAccountId = Guid.Parse("C12E22E4-DA76-4E51-AEAB-52B6575C7658");
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var username = HttpContext.Session.GetString("username");
            //var username = "ChesterSeda1";
            var postPhotoPath = "";

            if (postForm.Photo != null)
            {

                if (postForm.Photo.files.Length > 0)
                {
                    postPhotoPath = $"{username}\\posts\\{DateTime.Now.ToString("yyyyMMddhhmmss")}";
                    string path = $@"{_webHostEnvironment.WebRootPath}\{username}\postPicture\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + DateTime.Now.ToString("yyyyMMddhhmmss")))
                    {
                        postForm.Photo.files.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
           

            var newPost = new Post
            {
                UserAccountId = userAccountId,
                PostId = Guid.NewGuid(),
                TextContent = postForm.TextContent,
                TimeStamp = DateTime.Now,
                PostPhotoPath = postPhotoPath,
                PostLocation = postForm.PostLocation //GUID depends in a user's timeline
            };

            var add = await _postService.Insert(newPost);

            
            return StatusCode(StatusCodes.Status201Created, add);
        }

        [HttpDelete]
        [Route("/deletePost/{id:Guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var delete = await _postService.Delete(id);
            return StatusCode(StatusCodes.Status200OK, delete);
        }

        [HttpGet]
        [Route("newsfeed")]
        public async Task<IActionResult> GetNewsFeed()
        {

            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var friends = _friendService.GetFriendsId(userAccountId).ToList();

            friends.Add(userAccountId);

            var newsfeed = _postService.GetAllNewsfeedPost(friends);

            return StatusCode(StatusCodes.Status200OK, newsfeed);
        }

        [HttpGet]
        [Route("timeline")]
        public async Task<IActionResult> GetTimeline()
        {
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var posts = _postService.GetTimelinePosts(userAccountId);
            if(posts != null)
            {
                return StatusCode(StatusCodes.Status200OK, posts);
            }
            return StatusCode(StatusCodes.Status200OK, new Post());
        }


    }
}
