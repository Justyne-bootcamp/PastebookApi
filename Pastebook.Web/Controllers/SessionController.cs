using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Pastebook.Data.Models.DataTransferObjects;
using Pastebook.Web.Services;
using System;
using Pastebook.Web.Http;
using Pastebook.Web.DataTransferObjects;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        public SessionController(IUserAccountService userAccountService, ITokenGeneratorService tokenGeneratorService)
        {
            _userAccountService = userAccountService;
            _tokenGeneratorService = tokenGeneratorService;
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginFormDTO loginForm)
        {
            try
            {
                var user = _userAccountService.FindByEmail(loginForm.Email);
                var hashedPassword = _userAccountService.GetHashPassword(loginForm.Password, user.UserAccountId.ToString());
                if (user.Password.Equals(hashedPassword))
                {
                    HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetString("userAccountId", user.UserAccountId.ToString());

                    var token = _tokenGeneratorService.GenerateJwtToken(user.Username, user.UserAccountId.ToString());

                    return StatusCode(
                        StatusCodes.Status200OK,
                        new HttpResponseResult()
                        {
                            Message = token,
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
            catch(Exception e)
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new HttpResponseResult()
                    {
                        Message = e.Message,
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
