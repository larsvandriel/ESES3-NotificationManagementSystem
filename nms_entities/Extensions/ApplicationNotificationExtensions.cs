using NotificationManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.Extensions
{
    public static class ApplicationNotificationExtensions
    {
        public static void Map(this ApplicationNotification dbNotification, ApplicationNotification notification)
        {
            dbNotification.Title = notification.Title;
            dbNotification.Message = notification.Message;
            dbNotification.Sender = notification.Sender;
            dbNotification.Receivers = notification.Receivers;
            dbNotification.Viewed = notification.Viewed;
        }
    }
}
