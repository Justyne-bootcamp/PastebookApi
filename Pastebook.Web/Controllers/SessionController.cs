﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [Route("password/{password}")]
        public IActionResult LoginPassword(string password)
        {
            var email = HttpContext.Session.GetString("email");
            var user = _userAccountService.FindByEmail(email);
            if (user.Password.Equals(password))
            {
                var username = user.FirstName + user.LastName;
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetString("userAccountId", user.UserAccountId.ToString());
                return StatusCode(
                    StatusCodes.Status200OK,
                    new HttpResponseResult()
                    {
                        Message = username,
                        StatusCode = StatusCodes.Status200OK
                    });
            }
            else
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new HttpResponseResult()
                    {
                        Message = "Invalid Credential. Invalid Password.",
                        StatusCode = StatusCodes.Status404NotFound
                    });
            }
            
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("username");
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
