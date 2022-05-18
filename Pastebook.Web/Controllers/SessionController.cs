using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Pastebook.Web.Services;
using System;
using Pastebook.Web.Http;
using Pastebook.Web.DataTransferObjects;
using System.Threading.Tasks;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly IUserAccountService _userAccountService;
        public SessionController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginFormDTO loginForm)
        {
            try
            {   
                var user = _userAccountService.FindByEmail(loginForm.Email);
                var hashedPassword = _userAccountService.GetHashPassword(loginForm.Password, user.Username);

                if (user.Password.Equals(hashedPassword))
                {
                    HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetString("userAccountId", user.UserAccountId.ToString());

                    return StatusCode(StatusCodes.Status200OK,
                        new LoginResponse()
                        {
                            UserAccountId = user.UserAccountId.ToString(),
                            Username = user.Username,
                        }
                    );
                }
                else
                {
                    return StatusCode(
                        StatusCodes.Status404NotFound,
                        new HttpResponseError()
                        {
                            Message = "Invalid Credential. Invalid Password.",
                            StatusCode = StatusCodes.Status404NotFound
                        });
                }

            }
            catch(Exception e)
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new HttpResponseResult()
                    {
                        Message = "fail in try catch",
                        StatusCode = StatusCodes.Status404NotFound
                    });
            }
        }
        //for stage only
        [HttpPost]
        [Route("password/{password}")]
        public IActionResult LoginPassword(string password)
        {
            var email = HttpContext.Session.GetString("email");
            var user = _userAccountService.FindByEmail(email);
            var hashedPassword = _userAccountService.GetHashPassword(password, user.UserAccountId.ToString());
            if (user.Password.Equals(hashedPassword))
            {
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("userAccountId", user.UserAccountId.ToString());
                return StatusCode(
                    StatusCodes.Status200OK,
                    new HttpResponseResult()
                    {
                        Message = user.Username,
                        StatusCode = StatusCodes.Status200OK
                    });
            }
            else
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new HttpResponseError()
                    {
                        Message = "Invalid Credential. Invalid Password.",
                        StatusCode = StatusCodes.Status404NotFound
                    });
            }
            
        }

        [HttpPost]
        [Route("settingPassword")]
        public async Task<IActionResult> SettingPassword([FromBody] string password)
        {
            //var userAccountId = HttpContext.Session.GetString("userAccountId");
            var userAccountId = Guid.Parse("610D9455-4E7B-4B78-A4CF-99E47A48FCBE");
            var user = await _userAccountService.FindById(userAccountId);
            var hashedPassword = _userAccountService.GetHashPassword(password, user.Username);
            if (user.Password.Equals(hashedPassword))
            {
                return StatusCode(
                    StatusCodes.Status200OK,
                    new HttpResponseResult()
                    {
                        Message = user.Username,
                        StatusCode = StatusCodes.Status200OK
                    });
            }
            else
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,

                    hashedPassword);
            }

        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("userAccountId");
            return StatusCode(
                    StatusCodes.Status200OK,
                    new HttpResponseResult()
                    {
                        Message = "succesful logout",
                        StatusCode = StatusCodes.Status200OK
                    });
        }
    }
}
