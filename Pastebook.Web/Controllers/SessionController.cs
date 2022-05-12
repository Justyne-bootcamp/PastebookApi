using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Pastebook.Data.Models.DataTransferObjects;
using Pastebook.Web.Services;
using System;
using Pastebook.Web.Http;

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
        [Route("email/{email}")]
        public IActionResult LoginEmail(string email)
        {
            try
            {
                var result = _userAccountService.FindEmail(email);
                HttpContext.Session.SetString("email", email);
                return StatusCode(
                    StatusCodes.Status200OK,
                    new HttpResponseResult()
                    {
                        Message = "True",
                        StatusCode = StatusCodes.Status200OK
                    });

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
            if (user.Password.Equals(password))
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
