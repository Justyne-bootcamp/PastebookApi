using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Exceptions;
using Pastebook.Data.Models;
using Pastebook.Data.Models.DataTransferObjects;
using Pastebook.Web.DataTransferObjects;
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
        public async Task<IActionResult> GetUserAccountBySessionId([FromQuery] string sessionId)
        {
            //var sessionId = HttpContext.Session.GetString("userAccountId");
            var userAccountId = sessionId;
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
            var newPassword = _userAccountService.GetHashPassword(userAccount.Password, userAccount.Username);
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
        public async Task<IActionResult> UpdateRegistrationInfo(UpdateRegistrationDTO updateRegistrationDTO)
        {
            //var sessionId = HttpContext.Session.GetString("userAccountId");
            var sessionId = updateRegistrationDTO.SessionId;
            if (sessionId != updateRegistrationDTO.UserAccountId.ToString())
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            
            if (ModelState.IsValid)
            {
                if (updateRegistrationDTO.Password.Length < 6)
                {
                    throw new InvalidRegistrationException("Password length should be more than 6");
                }
                var user = await _userAccountService.FindById(Guid.Parse(sessionId));
                var newPassword = _userAccountService.GetHashPassword(updateRegistrationDTO.Password, updateRegistrationDTO.Username);
                user.FirstName = updateRegistrationDTO.FirstName;
                user.LastName = updateRegistrationDTO.LastName;
                user.Email = updateRegistrationDTO.Email;
                user.Password = newPassword;
                user.MobileNumber = updateRegistrationDTO.MobileNumber;
                user.Birthday = updateRegistrationDTO.Birthday;
                user.Gender = updateRegistrationDTO.Gender;
                await _userAccountService.Update(user);
                return StatusCode(StatusCodes.Status202Accepted, user);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        //[HttpPost]
        //[Route("aboutme")]

        //public async Task<IActionResult> AddAboutMe([FromBody] ProfileFormDTO profileForm)
        //{

        //    //var userAccountId = HttpContext.Session.GetString("userAccountId");
        //    var userAccountId = "FF2E9BD8-37A7-4980-8FC2-43AE89BB7A8D";
        //    Guid userAccountIdGuid = Guid.Parse(userAccountId);
        //    var userAccount = await _userAccountService.FindById(userAccountIdGuid);
        //    userAccount.AboutMe = profileForm.AboutMe;
        //    var updateAboutMe = await _userAccountService.Update(userAccount);

        //    return StatusCode(StatusCodes.Status200OK, profileForm.AboutMe);
        //}

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("editprofile")]
        public async Task<IActionResult> EditProfile([FromForm] ProfileFormDTO profileFormDTO)
        {
            try
            {
                //var userAccountId = "143ADC00-3A53-4F86-A175-6DBF4DB36BF8";
                Guid userAccountIdGuid = Guid.Parse(profileFormDTO.UserAccountId);
                var userAccount = await _userAccountService.FindById(userAccountIdGuid);

                if (profileFormDTO.ProfilePicture != null)
                {
                    //var username = HttpContext.Session.GetString("username");
                    var username = userAccount.Username;
                    string path = $@"{_webHostEnvironment.ContentRootPath}\..\..\PastebookClient\src\assets\uploaded_photo\{username}\profilePicture\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + profileFormDTO.ProfilePicture.FileName))
                    {
                        await profileFormDTO.ProfilePicture.CopyToAsync(fileStream);
                        fileStream.Flush();
                    }


                    //var userAccountId = HttpContext.Session.GetString("userAccountId");
                   
                    
                    userAccount.ProfilePhotoPath = $@"{profileFormDTO.ProfilePicture.FileName}";
                    

                    
                }
                if (!string.IsNullOrEmpty(profileFormDTO.AboutMe))
                {
                    userAccount.AboutMe = profileFormDTO.AboutMe;
                }
                var updateProfile = await _userAccountService.Update(userAccount);
                return StatusCode(StatusCodes.Status200OK, updateProfile);

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
