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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddPost([FromForm] PostFormDTO postForm)
        {
            var userAccountId = postForm.SessionId;
            //var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            //var username = HttpContext.Session.GetString("username");
            var username = "testangu1";
            var postPhotoPath = "";
            var newPost = new Post();
            try
            {
                if(postForm.Photo is not null)
                {
                    var fileExtension = Path.GetExtension(postForm.Photo.FileName);
                    postPhotoPath = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}{fileExtension}";
                    string path = $@"{_webHostEnvironment.ContentRootPath}\..\..\PastebookClient\src\assets\uploaded_photo\{username}\posts\";
                    //checking if folder not exist then create it
                    if ((!Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var filename = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}{fileExtension}";
                    //getting file name and combine with path and save it
                    using (var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await postForm.Photo.CopyToAsync(fileStream);
                    }
                }

                newPost = new Post
                {
                    UserAccountId = userAccountId,
                    PostId = Guid.NewGuid(),
                    TextContent = postForm.TextContent,
                    TimeStamp = DateTime.Now,
                    PostPhotoPath = postPhotoPath,
                    PostLocation = userAccountId
                };

                var add = await _postService.Insert(newPost);





            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, newPost);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("pastebook.com/{profileUsername}")]
        public async Task<IActionResult> AddPostToProfile([FromRoute] string profileUsername, [FromForm] PostFormDTO postForm)
        {
            var userAccountId = Guid.Parse("39315C50-15B9-4D95-9772-6749A6066CAB");
            //var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            //var username = HttpContext.Session.GetString("username");
            var username = "testangu1";
            var postPhotoPath = "";
            var newPost = new Post();
            try
            {
                if (postForm.Photo is not null)
                {
                    var fileExtension = Path.GetExtension(postForm.Photo.FileName);
                    postPhotoPath = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}{fileExtension}";
                    string path = $@"{_webHostEnvironment.ContentRootPath}\..\..\PastebookClient\src\assets\uploaded_photo\{username}\posts\";
                    //checking if folder not exist then create it
                    if ((!Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var filename = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}{fileExtension}";
                    //getting file name and combine with path and save it
                    using (var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await postForm.Photo.CopyToAsync(fileStream);
                    }

                }

                var profileUser = _userAccountService.FindByUsername(profileUsername);
                newPost = new Post
                {
                    UserAccountId = userAccountId,
                    PostId = Guid.NewGuid(),
                    TextContent = postForm.TextContent,
                    TimeStamp = DateTime.Now,
                    PostPhotoPath = postPhotoPath,
                    PostLocation = profileUser.UserAccountId
                };

                var add = await _postService.Insert(newPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, newPost);
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
        public async Task<IActionResult> GetNewsFeed([FromQuery] string sessionId)
        {

            //var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var userAccountId = Guid.Parse(sessionId);
            var friends = _friendService.GetFriendsId(userAccountId).ToList();

            friends.Add(userAccountId);

            var newsfeed = _postService.GetAllNewsfeedPost(friends);

            return StatusCode(StatusCodes.Status200OK, newsfeed);
        }

        [HttpGet]
        [Route("timeline")]
        public async Task<IActionResult> GetTimeline([FromQuery] string sessionId)
        {
            //var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var userAccountId = Guid.Parse(sessionId);
            var posts = _postService.GetTimelinePosts(userAccountId);
            if(posts != null)
            {
                return StatusCode(StatusCodes.Status200OK, posts);
            }
            return StatusCode(StatusCodes.Status200OK, new Post());
        }


    }
}
