using NotificationManagementSystem.Entities.Models;
using NotificationManagementSystem.Entities.ShapedEntities;

namespace NotificationManagementSystem.Contracts
{
    public interface INotificationRepository: IRepositoryBase<Notification>
    {
        ShapedEntity GetNotificationById(Guid notificationId, string fields);
        Notification GetNotificationById(Guid notificationId);
        void CreateNotification(Notification notification);
        void UpdateNotification(Notification dbNotification, Notification notification);
        void DeleteNotification(Notification notification);
    }
}