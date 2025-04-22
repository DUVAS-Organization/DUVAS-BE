using DTO;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class NotificationRepository : INotificationRepository
    {
        public async Task<List<NotificationDTO>> GetNotificationsByUserAsync(int userId)
        {
            return await NotificationDAO.GetNotificationsByUserAsync(userId);
        }

        public async Task<List<NotificationDTO>> GetNotificationUnreadByUserAsync(int userId)
        {
            return await NotificationDAO.GetNotificationUnreadByUserAsync(userId);
        }

        public async Task<List<NotificationDTO>> GetAllNotificationsAsync()
        {
            return await NotificationDAO.GetAllNotificationsAsync();
        }

        public async Task<List<NotificationDTO>> GetNotificationsByTypeAsync(string type)
        {
            return await NotificationDAO.GetNotificationsByTypeAsync(type);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            await NotificationDAO.MarkAsReadAsync(notificationId);
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            await NotificationDAO.DeleteNotificationAsync(notificationId);
        }

        public async Task<int> CountUnreadNotificationsAsync(int userId)
        {
            return await NotificationDAO.CountUnreadNotificationsAsync(userId);
        }
    }
}