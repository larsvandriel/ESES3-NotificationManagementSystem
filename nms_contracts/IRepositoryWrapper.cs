using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Contracts
{
    public interface IRepositoryWrapper
    {
        INotificationRepository Notification { get; }
        void Save();
    }
}
