﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
                var hashedPassword = _userAccountService.GetHashPassword(loginForm.Password, user.UserAccountId.ToString());
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
    }
}
