using NotificationManagementSystem.Entities.Helpers;
using NotificationManagementSystem.Entities.Models;
using NotificationManagementSystem.Entities.Parameters;
using NotificationManagementSystem.Entities.ShapedEntities;

namespace NotificationManagementSystem.Contracts
{
    public interface INotificationRepository: IRepositoryBase<Notification>
    {
        PagedList<ShapedEntity> GetAllNotifications(NotificationParameters notificationParameters);
        ShapedEntity GetNotificationById(Guid notificationId, string fields);
        Notification GetNotificationById(Guid notificationId);
        void CreateNotification(Notification notification);
        void UpdateNotification(Notification dbNotification, Notification notification);
        void DeleteNotification(Notification notification);
    }
}