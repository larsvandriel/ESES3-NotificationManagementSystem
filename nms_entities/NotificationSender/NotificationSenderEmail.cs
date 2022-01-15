using NotificationManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.NotificationSender
{
    public class NotificationSenderEmail : INotificationSender
    {
        public NetworkCredential Credential { get; set; }

        public NotificationSenderEmail(NetworkCredential credential)
        {
            Credential = credential;
        }

        public void SendNotification(Notification notification)
        {
            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress(notification.Sender.Email);
            foreach(var recipient in notification.Receivers)
            {
                Msg.To.Add(new MailAddress(recipient.Email));
            }
            Msg.Subject = notification.Title;
            Msg.Body = notification.Message;

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.live.com";
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = Credential;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(Msg);
        }
    }
}
