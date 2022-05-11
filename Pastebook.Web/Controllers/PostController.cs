using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.Http;
using Pastebook.Web.Models;
using Pastebook.Web.Services;
using System;
using System.IO;
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

        public PostController(IPostService PostService, IUserAccountService userAccountService, IWebHostEnvironment webHostEnvironment)
        {
            _postService = PostService;
            _userAccountService = userAccountService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("/upload/{objectFile}")]
        public string Upload([FromForm] FileUpload objectFile)
        {
            try
            {
                if (objectFile.files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + objectFile.files.FileName))
                    {
                        objectFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "Uploaded";
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return "Okay";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var post = await _postService.FindAll();
            return StatusCode(StatusCodes.Status200OK, post);
        }

        [HttpPost]
        [Route("/addPost/{post}")]
        public async Task<IActionResult> AddPost(Post post)
        {
            //var post = new Post
            //{
            //    UserAccountId = Guid.Parse("F5D50E0B-D82C-4E06-BA17-BC142151CA5B"),
            //    PostId = Guid.NewGuid(),
            //    TextContent = "i am mikko ",
            //    TimeStamp = DateTime.Now,
            //    PostPhotoPath = "mikko.dacasin.jpg",
            //};

            var add = await _postService.Insert(post);
            return StatusCode(StatusCodes.Status201Created, add);
        }

        [HttpDelete]
        [Route("/deletePost/{id:Guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var delete = await _postService.Delete(id);
            return StatusCode(StatusCodes.Status200OK, delete);
        } 
    }
}
