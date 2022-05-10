using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.Data;
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
        private IUnitOfWork UnitOfWork { get; set; }
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

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                var newUser = await UnitOfWork.UserAccountRepository.Insert(userAccount);
                await UnitOfWork.CommitAsync();

                return StatusCode(StatusCodes.Status201Created, newUser);
            }

            return BadRequest();
        }
    }
}
