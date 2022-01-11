using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.Models
{
    public abstract class Notification
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Employee Sender { get; set; }
        public List<Employee> Receivers { get; set; }
    }
}
