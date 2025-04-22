using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface INotificationRepository
    {
        Task<List<NotificationDTO>> GetNotificationsByUserAsync(int userId);
        Task<List<NotificationDTO>> GetNotificationUnreadByUserAsync(int userId);
        Task<List<NotificationDTO>> GetAllNotificationsAsync();
        Task<List<NotificationDTO>> GetNotificationsByTypeAsync(string type);
        Task MarkAsReadAsync(int notificationId);
        Task DeleteNotificationAsync(int notificationId);
        Task<int> CountUnreadNotificationsAsync(int userId);
    }
}