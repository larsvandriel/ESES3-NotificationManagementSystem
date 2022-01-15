using NotificationManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.NotificationSender
{
    public interface INotificationSender
    {
        void SendNotification(Notification notification);
    }
}
