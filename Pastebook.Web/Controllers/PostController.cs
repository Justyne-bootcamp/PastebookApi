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
            var getUsername = await _userAccountService.FindById(userAccountId);
            var username = getUsername.Username;
            var postPhotoPath = "";
            var photoPath = "";
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
                    photoPath = $@"{ username }/posts/{ postPhotoPath }";
                }

                newPost = new Post
                {
                    UserAccountId = userAccountId,
                    PostId = Guid.NewGuid(),
                    TextContent = postForm.TextContent,
                    TimeStamp = DateTime.Now,
                    PostPhotoPath = photoPath,
                    PostLocation = userAccountId
                };

                var add = await _postService.Insert(newPost);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }

            return StatusCode(StatusCodes.Status201Created, newPost);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("posttoprofile")]
        public async Task<IActionResult> AddPostToProfile([FromForm] PostFormDTO postForm)
        {
            var userAccountId = postForm.SessionId;
            var username = postForm.Username;
            var postPhotoPath = "";
            var photoPath = "";
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
                    photoPath = $@"{ username }/posts/{ postPhotoPath }";
                }

                var profileUser = _userAccountService.FindByUsername(postForm.Username);
                newPost = new Post
                {
                    UserAccountId = userAccountId,
                    PostId = Guid.NewGuid(),
                    TextContent = postForm.TextContent,
                    TimeStamp = DateTime.Now,
                    PostPhotoPath = photoPath,
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
            var userAccountId = Guid.Parse(sessionId);
            var friends = _friendService.GetFriendsId(userAccountId).ToList();

            friends.Add(userAccountId);

            var newsfeed = _postService.GetAllNewsfeedPost(userAccountId, friends);

            return StatusCode(StatusCodes.Status200OK, newsfeed);
        }

        [HttpGet]
        [Route("timeline")]
        public async Task<IActionResult> GetTimeline([FromQuery] string username, string sessionId)
        {

            var profileUser = _userAccountService.FindByUsername(username);
            var userAccountId = profileUser.UserAccountId;
            var posts = _postService.GetTimelinePosts(userAccountId, sessionId, username);
            if(posts != null)
            {
                return StatusCode(StatusCodes.Status200OK, posts);
            }
            return StatusCode(StatusCodes.Status200OK, new Post());
        }


    }
}
