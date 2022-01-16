using NotificationManagementSystem.Contracts;
using NotificationManagementSystem.Entities;
using NotificationManagementSystem.Entities.Extensions;
using NotificationManagementSystem.Entities.Helpers;
using NotificationManagementSystem.Entities.Models;
using NotificationManagementSystem.Entities.Parameters;
using NotificationManagementSystem.Entities.ShapedEntities;

namespace NotificationManagementSystem.Repository
{
    public class NotificationRepository: RepositoryBase<Notification>, INotificationRepository
    {
        private readonly ISortHelper<Notification> _sortHelper;

        private readonly IDataShaper<Notification> _dataShaper;

        public NotificationRepository(RepositoryContext repositoryContext, ISortHelper<Notification> sortHelper, IDataShaper<Notification> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateNotification(Notification notification)
        {
            Create(notification);
        }

        public void DeleteNotification(Notification notification)
        {
            Delete(notification);
        }

        public PagedList<ShapedEntity> GetAllNotifications(NotificationParameters notificationParameters)
        {
            var notifications = FindAll();

            SearchByTitle(ref notifications, notificationParameters.Title);

            var sortedNotifications = _sortHelper.ApplySort(notifications, notificationParameters.OrderBy);
            var shapedNotifications = _dataShaper.ShapeData(sortedNotifications, notificationParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedNotifications, notificationParameters.PageNumber, notificationParameters.PageSize);
        }

        public ShapedEntity GetNotificationById(Guid notificationId, string fields)
        {
            var notification = FindByCondition(notification => notification.Id.Equals(notificationId)).FirstOrDefault();

            if (notification == null)
            {
                notification = new ApplicationNotification();
            }

            return _dataShaper.ShapeData(notification, fields);
        }

        public Notification GetNotificationById(Guid notificationId)
        {
            return FindByCondition(i => i.Id.Equals(notificationId)).FirstOrDefault();
        }

        public void UpdateNotification(Notification dbNotification, Notification notification)
        {
            if(dbNotification is EmailNotification)
            {
                ((EmailNotification)dbNotification).Map((EmailNotification)notification);
            }
            if(dbNotification is ApplicationNotification)
            {
                ((ApplicationNotification)dbNotification).Map((ApplicationNotification)notification);
            }
            
            Update(dbNotification);
        }

        private void SearchByTitle(ref IQueryable<Notification> notifications, string notificationTitle)
        {
            if (!notifications.Any() || string.IsNullOrWhiteSpace(notificationTitle))
            {
                return;
            }

            notifications = notifications.Where(i => i.Title.ToLower().Contains(notificationTitle.Trim().ToLower()));
        }
    }
}