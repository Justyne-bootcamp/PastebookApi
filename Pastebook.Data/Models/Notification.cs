using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Models
{
    public class Notification: BaseEntity
    {
        public Guid NotificationId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string NotificationStatus { get; set; }
        public string NotificationType { get; set; }
        public string NotificationPath { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid NotificationSourceId { get; set; }
    }
}
