using Pastebook.Data.Models;
using Pastebook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface INotificationService
    {
        public Task<Notification> Insert(Notification notification);
        public IEnumerable<Notification> GetAllUserNotifications(Guid userAccountId);
        public IEnumerable<Notification> SetToReadAllNotifications(Guid userAccountId);
    }
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<Notification> GetAllUserNotifications(Guid userAccountId)
        {
            return _notificationRepository.GetAllUserNotifications(userAccountId);
        }

        public Task<Notification> Insert(Notification notification)
        {
            return _notificationRepository.Insert(notification);
        }

        public IEnumerable<Notification> SetToReadAllNotifications(Guid userAccountId)
        {
            return _notificationRepository.SetToReadAllNotifications(userAccountId);
        }
    }
}
