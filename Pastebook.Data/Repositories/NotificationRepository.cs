using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Repositories
{
    public interface INotificationRepository: IBaseRepository<Notification>
    {
        public IEnumerable<Notification> GetAllUserNotifications(Guid userAccountId);
        public IEnumerable<Notification> SetToReadAllNotifications(Guid userAccountId);
    }
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(PastebookContext context) : base(context)
        {
        }

        public IEnumerable<Notification> GetAllUserNotifications(Guid userAccountId)
        {
            var unreadNotification = this.Context.Notifications
                .Where(notification => notification.UserAccountId.Equals(userAccountId) && notification.NotificationStatus.Equals("Unread"))
                .ToList();
            return unreadNotification;
        }

        public IEnumerable<Notification> SetToReadAllNotifications(Guid userAccountId)
        {
            var readNotification = GetAllUserNotifications(userAccountId);
            foreach(var notification in readNotification)
            {
                notification.NotificationStatus = "Read";
            }
            this.Context.SaveChanges();
            return readNotification;
        }
    }
}
