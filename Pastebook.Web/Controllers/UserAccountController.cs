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
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserAccountController(IUserAccountService userAccountService, IWebHostEnvironment webHostEnvironment)
        {
            _userAccountService = userAccountService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userAccount = await _userAccountService.FindAll();
            return StatusCode(StatusCodes.Status200OK, userAccount);
        }

        [HttpPost]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUserAccount(Guid id)
        {
            try
            {
                var userAccount = await _userAccountService.FindById(id);
                return StatusCode(StatusCodes.Status200OK, userAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(
                StatusCodes.Status404NotFound,
                new HttpResponseError()
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status404NotFound
                }
             );
            }
        }

        [HttpPost, Route("/register")]
        public IActionResult RegisterNewUser(UserAccount userAccount)
        {
            var newGuid = Guid.NewGuid();
            userAccount.UserAccountId = newGuid;
            userAccount.Username = _userAccountService.CreateUsername(userAccount.FirstName, userAccount.LastName);
            var newPassword = _userAccountService.GetHashPassword(userAccount.Password, newGuid.ToString());
            userAccount.Password = newPassword;
            var newUser = _userAccountService.Insert(userAccount);
            return StatusCode(StatusCodes.Status201Created, newUser);
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> RegisterNewUser([FromForm] UserAccount userAccount)
        {
            if (userAccount.Password.Length < 6)
            {
                throw new InvalidRegistrationException("Password length should be more than 6");
            }
            if (_userAccountService.FindEmail(userAccount.Email))
            {
                throw new InvalidRegistrationException("Email already in used. Try a different email.");
            }
            var newGuid = Guid.NewGuid();
            userAccount.UserAccountId = newGuid;
            userAccount.Username = _userAccountService.CreateUsername(userAccount.FirstName, userAccount.LastName);
            var newPassword = _userAccountService.GetHashPassword(userAccount.Password, newGuid.ToString());
            userAccount.Password = newPassword;
            var newUser = await _userAccountService.Insert(userAccount);
            return StatusCode(StatusCodes.Status201Created, newUser);
        }

        [HttpGet, Route("search")]
        public async Task<IActionResult> SearchName([FromQuery] string searchName)
        {
            if (string.IsNullOrWhiteSpace(Request.Query["searchName"]))
            {
                var usersAllFound = await _userAccountService.FindAll();
                return StatusCode(StatusCodes.Status200OK, usersAllFound);
            }
            var usersFound = _userAccountService.FindByName(searchName);
            return StatusCode(StatusCodes.Status200OK, usersFound);
        }

        [HttpPut, Route("setting")]
        public async Task<IActionResult> UpdateRegistrationInfo(UserAccount userAccount)
        {
            var sessionId = HttpContext.Session.GetString("userAccountId");
            if (sessionId != userAccount.UserAccountId.ToString())
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            if (ModelState.IsValid)
            {
                if (userAccount.Password.Length < 6)
                {
                    throw new InvalidRegistrationException("Password length should be more than 6");
                }
                await _userAccountService.Update(userAccount);
                return StatusCode(StatusCodes.Status202Accepted, userAccount);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPut]
        [Route("aboutme/{aboutMe}")]

        public async Task<IActionResult> AddAboutMe(string aboutMe)
        {

            var userAccountId = HttpContext.Session.GetString("userAccountId");
            Guid userAccountIdGuid = Guid.Parse(userAccountId);
            var userAccount = await _userAccountService.FindById(userAccountIdGuid);
            userAccount.AboutMe = aboutMe;
            var updateAboutMe = await _userAccountService.Update(userAccount);

            return StatusCode(StatusCodes.Status200OK, updateAboutMe);
        }

       
        [HttpPost]
        [Route("profilepicture")]
        public async Task<IActionResult> Upload([FromForm] FileUpload profilePicture)
        {
            try
            {
                if (profilePicture.files.Length > 0)
                {
                    var username = HttpContext.Session.GetString("username");
                    string path = $@"{_webHostEnvironment.WebRootPath}\{username}\profilePicture\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + profilePicture.files.FileName))
                    {
                        profilePicture.files.CopyTo(fileStream);
                        fileStream.Flush();
                    }


                    var userAccountId = HttpContext.Session.GetString("userAccountId");
                    Guid userAccountIdGuid = Guid.Parse(userAccountId);
                    var userAccount = await _userAccountService.FindById(userAccountIdGuid);
                    userAccount.ProfilePhotoPath = $@"\wwwRoot\{username}\profilePicture\{profilePicture.files.FileName}";
                    var updateProfilePicture = await _userAccountService.Update(userAccount);
                    return StatusCode(StatusCodes.Status200OK, updateProfilePicture);
                }

                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new HttpResponseError()
                    {
                        Message = "No image found.",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new HttpResponseError()
                    {
                        Message = "Upload Failed.",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
            }
        }
    }
}
