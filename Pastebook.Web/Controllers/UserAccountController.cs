using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Exceptions;
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
        public async Task<IActionResult> GetAllUsers()
        {
            var userAccount = await _userAccountService.FindAll();
            return StatusCode(StatusCodes.Status200OK, userAccount);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetUserAccount(string username)
        {
            try
            {
                var userAccount = _userAccountService.FindByUsername(username);
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

        [HttpGet]
        [Route("setting")]
        public async Task<IActionResult> GetUserAccountBySessionId()
        {
            //var sessionId = HttpContext.Session.GetString("userAccountId");
            var sessionId = "F2817916-455F-41DB-A869-17BA5A733ADF";
            try
            {
                var userAccount = await _userAccountService.FindById(Guid.Parse(sessionId));
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

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser([FromBody] UserAccount userAccount)
        {
            if (userAccount.Password.Length < 6)
            {
                throw new InvalidRegistrationException("Password length should be more than 6");
            }
            var newGuid = Guid.NewGuid();
            userAccount.UserAccountId = newGuid;
            userAccount.Username = _userAccountService.CreateUsername(userAccount.FirstName, userAccount.LastName);
            var newPassword = _userAccountService.GetHashPassword(userAccount.Password, newGuid.ToString());
            userAccount.Password = newPassword;
            var newUser = await _userAccountService.Insert(userAccount);
            return StatusCode(StatusCodes.Status201Created, newUser);
        }

        [HttpGet, Route("search/{searchName}")]
        public async Task<IActionResult> SearchName(string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
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
            //var sessionId = HttpContext.Session.GetString("userAccountId");
            var sessionId = "F2817916-455F-41DB-A869-17BA5A733ADF";
            if (sessionId.ToLower() != userAccount.UserAccountId.ToString())
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            
            if (ModelState.IsValid)
            {
                if (userAccount.Password.Length < 6)
                {
                    throw new InvalidRegistrationException("Password length should be more than 6");
                }
                var user = await _userAccountService.FindById(Guid.Parse(sessionId));
                userAccount.Username = user.Username;
                var newPassword = _userAccountService.GetHashPassword(userAccount.Password, sessionId);
                userAccount.Password = newPassword;
                await _userAccountService.Update(userAccount);
                return StatusCode(StatusCodes.Status202Accepted, userAccount);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPut]
        [Route("aboutme")]

        public async Task<IActionResult> AddAboutMe([FromForm] string aboutMe)
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
