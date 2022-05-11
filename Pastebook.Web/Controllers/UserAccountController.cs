using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var newGuid = Guid.NewGuid();
            userAccount.UserAccountId = newGuid;
            userAccount.Username = _userAccountService.CreateUsername(userAccount.FirstName, userAccount.LastName);
            var newPassword = _userAccountService.GetHashPassword(userAccount.Password,newGuid.ToString());
            userAccount.Password = newPassword;
            var newUser = await _userAccountService.Insert(userAccount);
            return StatusCode(StatusCodes.Status201Created, newUser);
        }

        [HttpGet, Route("/search")]
        public IActionResult SearchName([FromQuery] string searchName)
        {
            var usersFound = _userAccountService.FindByName(searchName);
            return StatusCode(StatusCodes.Status200OK, usersFound);
        }

        [HttpPost, Route("/setting/{id:Guid}")]
        public async Task<IActionResult> UpdateRegistrationInfo([FromRoute]Guid id, UserAccount userAccount)
        {
            if(id != userAccount.UserAccountId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _userAccountService.Update(userAccount);
                return StatusCode(StatusCodes.Status202Accepted, userAccount);
            }
            return BadRequest();
        }
    }
}
