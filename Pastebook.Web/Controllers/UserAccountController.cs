using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Exceptions;
using Pastebook.Data.Models;
using Pastebook.Web.Http;
using Pastebook.Web.Services;
using System;
using System.Threading.Tasks;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        public UserAccountController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
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
            }catch (Exception ex)
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
        public async Task<IActionResult> RegisterNewUser([FromForm] UserAccount userAccount)
        {
            if(userAccount.Password.Length < 6)
            {
                throw new InvalidRegistrationException("Password length should be more than 6");
            }
            if(_userAccountService.FindEmail(userAccount.Email))
            {
                throw new InvalidRegistrationException("Email already in used. Try a different email.");
            }
            var newGuid = Guid.NewGuid();
            userAccount.UserAccountId = newGuid;
            userAccount.Username = _userAccountService.CreateUsername(userAccount.FirstName, userAccount.LastName);
            var newPassword = _userAccountService.GetHashPassword(userAccount.Password,newGuid.ToString());
            userAccount.Password = newPassword;
            var newUser = await _userAccountService.Insert(userAccount);
            return StatusCode(StatusCodes.Status201Created, newUser);
        }

        [HttpGet, Route("/search")]
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

        [HttpPut, Route("/setting")]
        public async Task<IActionResult> UpdateRegistrationInfo(UserAccount userAccount)
        {
            var sessionId = HttpContext.Session.GetString("userAccountId");
            if(sessionId != userAccount.UserAccountId.ToString())
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
    }
}
