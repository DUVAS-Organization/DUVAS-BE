using BusinessObject;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class NotificationDAO
    {
        public static async Task<List<NotificationDTO>> GetNotificationsByUserAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Notifications
                        .Where(n => n.UserId == userId)
                        .Select(n => new NotificationDTO
                        {
                            NotificationId = n.NotificationId,
                            UserId = n.UserId,
                            Type = n.Type,
                            Message = n.Message,
                            IsRead = n.IsRead,
                            CreatedDate = n.CreatedDate.ToString("HH:mm - dd/MM/yyyy")
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông báo theo user: {ex.Message}");
            }
        }

        public static async Task<List<NotificationDTO>> GetNotificationUnreadByUserAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Notifications
                        .Where(n => n.UserId == userId && !n.IsRead)
                        .Select(n => new NotificationDTO
                        {
                            NotificationId = n.NotificationId,
                            UserId = n.UserId,
                            Type = n.Type,
                            Message = n.Message,
                            IsRead = n.IsRead,
                            CreatedDate = n.CreatedDate.ToString("HH:mm - dd/MM/yyyy")
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông báo chưa đọc: {ex.Message}");
            }
        }

        public static async Task<List<NotificationDTO>> GetAllNotificationsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Notifications
                        .Select(n => new NotificationDTO
                        {
                            NotificationId = n.NotificationId,
                            UserId = n.UserId,
                            Type = n.Type,
                            Message = n.Message,
                            IsRead = n.IsRead,
                            CreatedDate = n.CreatedDate.ToString("HH:mm - dd/MM/yyyy")
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy tất cả thông báo: {ex.Message}");
            }
        }

        public static async Task<List<NotificationDTO>> GetNotificationsByTypeAsync(string type)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Notifications
                        .Where(n => n.Type == type)
                        .Select(n => new NotificationDTO
                        {
                            NotificationId = n.NotificationId,
                            UserId = n.UserId,
                            Type = n.Type,
                            Message = n.Message,
                            IsRead = n.IsRead,
                            CreatedDate = n.CreatedDate.ToString("HH:mm - dd/MM/yyyy")
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông báo theo loại: {ex.Message}");
            }
        }

        public static async Task MarkAsReadAsync(int notificationId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var notification = await context.Notifications
                        .FirstOrDefaultAsync(n => n.NotificationId == notificationId);

                    if (notification == null)
                        throw new Exception("Thông báo không tồn tại.");

                    notification.IsRead = true;
                    context.Entry(notification).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đánh dấu thông báo đã đọc: {ex.Message}");
            }
        }

        public static async Task DeleteNotificationAsync(int notificationId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var notification = await context.Notifications
                        .FirstOrDefaultAsync(n => n.NotificationId == notificationId);

                    if (notification == null)
                        throw new Exception("Thông báo không tồn tại.");

                    context.Notifications.Remove(notification);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa thông báo: {ex.Message}");
            }
        }

        public static async Task<int> CountUnreadNotificationsAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Notifications
                        .CountAsync(n => n.UserId == userId && !n.IsRead);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm thông báo chưa đọc: {ex.Message}");
            }
        }
    }
}