using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.Models
{
    public class ApplicationNotification: Notification
    {
        public bool Viewed { get; set; }
    }
}
