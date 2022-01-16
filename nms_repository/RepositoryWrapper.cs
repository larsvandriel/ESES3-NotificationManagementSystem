using NotificationManagementSystem.Contracts;
using NotificationManagementSystem.Entities;
using NotificationManagementSystem.Entities.Helpers;
using NotificationManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;

        private INotificationRepository _notification;
        private ISortHelper<Notification> _notificationSortHelper;
        private IDataShaper<Notification> _notificationDataShaper;

        public INotificationRepository Notification
        {
            get
            {
                if (_notification == null)
                {
                    _notification = new NotificationRepository(_repoContext, _notificationSortHelper, _notificationDataShaper);
                }

                return _notification;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext, ISortHelper<Notification> notificationSortHelper, IDataShaper<Notification> notificationDataShaper)
        {
            _repoContext = repositoryContext;
            _notificationSortHelper = notificationSortHelper;
            _notificationDataShaper = notificationDataShaper;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
