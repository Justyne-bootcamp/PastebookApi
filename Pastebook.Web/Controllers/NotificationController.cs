using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pastebook.Data.Models;
using Pastebook.Web.Services;
using System;

namespace Pastebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("notifications")]
        public IActionResult GetAllUserNotifications()
        {
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var notifications = _notificationService.GetAllUserNotifications(userAccountId);
            if(notifications != null)
            {
                return StatusCode(StatusCodes.Status200OK, notifications);
            }
            return StatusCode(StatusCodes.Status200OK, new Notification());
        }

        [HttpGet]
        [Route("readnotifications")]
        public IActionResult SetToReadAllNotifications()
        {
            var userAccountId = Guid.Parse(HttpContext.Session.GetString("userAccountId"));
            var notifications = _notificationService.SetToReadAllNotifications(userAccountId);
            
            return StatusCode(StatusCodes.Status200OK, notifications);
        }
    }
}
