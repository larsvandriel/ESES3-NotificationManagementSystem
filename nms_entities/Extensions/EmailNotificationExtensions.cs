using NotificationManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.Extensions
{
    public static class EmailNotificationExtensions
    {
        public static void Map(this EmailNotification dbNotification, EmailNotification notification)
        {
            dbNotification.Title = notification.Title;
            dbNotification.Message = notification.Message;
            dbNotification.Sender = notification.Sender;
            dbNotification.Receivers = notification.Receivers;
            dbNotification.TimeSend = notification.TimeSend;
        }
    }
}
