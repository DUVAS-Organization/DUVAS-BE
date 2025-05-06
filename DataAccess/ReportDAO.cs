using BusinessObject;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ReportDAO
    {
        private readonly ApplicationDbContext _context;

        public ReportDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<ReportDTO>> GetReportsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var reports = await context.Reports
                        .AsNoTracking()
                        .Select(p => new ReportDTO
                        {
                            ReportId = p.ReportId,
                            UserId = p.UserId,
                            RoomId = p.RoomId,
                            ReportContent = p.ReportContent,
                            Image = p.Image,
                            Status = p.Status,
                            Feedback = p.Feedback,
                            CreatedTime = p.CreatedTime // Thêm dòng này
                        })
                        .ToListAsync();

                    return reports;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<Report> FindReportByIdAsync(int reportId)
        {
            Report report = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    report = await context.Reports.SingleOrDefaultAsync(x => x.ReportId == reportId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return report;
        }

        public static async Task SaveReportAsync(Report report)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.Reports.AddAsync(report);
                    await context.SaveChangesAsync();

                    // ✅ Gửi thông báo cho admin hoặc user (tuỳ mục đích)
                    var notification = new Notification
                    {
                        UserId = report.UserId, // hoặc gán ID admin nếu muốn admin nhận
                        Type = "NewReport",
                        Message = $"Báo cáo mới đã được gửi.",
                        RedirectUrl = "/reports",
                        CreatedDate = DateTime.Now,
                        IsRead = false
                    };

                    await NotificationDAO.CreateNotificationAsync(notification);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateReportAsync(Report report)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(report).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteReportAsync(Report report)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingReport = await context.Reports.SingleOrDefaultAsync(c => c.ReportId == report.ReportId);
                    if (existingReport != null)
                    {
                        context.Reports.Remove(existingReport);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<ReportDTO>> GetReportsByUserIdAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var reports = await context.Reports
                        .AsNoTracking()
                        .Where(r => r.UserId == userId)
                        .Select(p => new ReportDTO
                        {
                            ReportId = p.ReportId,
                            UserId = p.UserId,
                            RoomId = p.RoomId,
                            ReportContent = p.ReportContent,
                            Image = p.Image,
                            Status = p.Status,
                            Feedback = p.Feedback,
                            CreatedTime = p.CreatedTime
                        })
                        .ToListAsync();

                    return reports;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<ReportDTO>> GetPendingReportsByUserAndRoomAsync(int userId, int? roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var reports = await context.Reports
                        .AsNoTracking()
                        .Where(r => r.UserId == userId &&
                                   r.RoomId == roomId &&
                                   r.Status == 0) // Chỉ lấy các report chưa xử lý
                        .Select(p => new ReportDTO
                        {
                            ReportId = p.ReportId,
                            UserId = p.UserId,
                            RoomId = p.RoomId,
                            ReportContent = p.ReportContent,
                            Image = p.Image,
                            Status = p.Status,
                            Feedback = p.Feedback,
                            CreatedTime = p.CreatedTime
                        })
                        .ToListAsync();

                    return reports;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<bool> HasPendingReport(int userId, int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Reports
                        .AnyAsync(r => r.UserId == userId &&
                                     r.RoomId == roomId &&
                                     r.Status == 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<ReportDTO>> GetReportsByLandlordIdAsync(int landlordId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var reports = await context.Reports
                        .AsNoTracking()
                        .Join(context.Rooms,
                            report => report.RoomId,
                            room => room.RoomId,
                            (report, room) => new { Report = report, Room = room })
                        .Where(joined => joined.Room.UserId == landlordId)
                        .Select(joined => new ReportDTO
                        {
                            ReportId = joined.Report.ReportId,
                            UserId = joined.Report.UserId,
                            RoomId = joined.Report.RoomId,
                            RoomTitle = joined.Room.Title, // Thêm trường RoomTitle
                            ReportContent = joined.Report.ReportContent,
                            Image = joined.Report.Image,
                            Status = joined.Report.Status,
                            Feedback = joined.Report.Feedback,
                            CreatedTime = joined.Report.CreatedTime // Thêm CreatedTime nếu chưa có
                        })
                        .ToListAsync();

                    return reports;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<int> GetRoomOwnerIdAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var room = await context.Rooms
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.RoomId == roomId);

                    return room?.UserId ?? 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task LockRoomAsync(int reportId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    // Tìm báo cáo
                    var report = await context.Reports
                        .FirstOrDefaultAsync(r => r.ReportId == reportId);
                    if (report == null)
                    {
                        throw new Exception("Báo cáo không tồn tại.");
                    }

                    // Kiểm tra trạng thái báo cáo
                    if (report.Status != 0)
                    {
                        throw new Exception("Báo cáo đã được xử lý, không thể khóa phòng.");
                    }

                    report.Status = 2;
                    context.Entry(report).State = EntityState.Modified;

                    int? roomId = report.RoomId;
                    if (!roomId.HasValue)
                    {
                        throw new Exception("Báo cáo không có RoomId liên kết.");
                    }

                    var room = await context.Rooms
                        .FirstOrDefaultAsync(r => r.RoomId == roomId.Value);
                    if (room == null)
                    {
                        throw new Exception("Phòng không tồn tại.");
                    }
                    room.IsPermission = 2;
                    context.Entry(room).State = EntityState.Modified;

                    // Gửi thông báo cho người báo cáo
                    var userNotification = new Notification
                    {
                        UserId = report.UserId,
                        Type = "RoomLocked",
                        Message = "Phòng bạn báo cáo đã được khóa",
                        RedirectUrl = "/Room",
                        CreatedDate = DateTime.Now,
                        IsRead = false
                    };

                    // Gửi thông báo cho chủ phòng
                    var landlordNotification = new Notification
                    {
                        UserId = room.UserId,
                        Type = "RoomLockedByAdmin",
                        Message = $"Phòng ở {room.LocationDetail} của bạn đã bị Admin khóa",
                        RedirectUrl = "/Room",
                        CreatedDate = DateTime.Now,
                        IsRead = false
                    };

                    await context.Notifications.AddRangeAsync(new List<Notification> { userNotification, landlordNotification });

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi khóa phòng: {ex.Message}");
            }
        }
        public static async Task LockAccountAsync(int reportId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var report = await context.Reports
                        .FirstOrDefaultAsync(r => r.ReportId == reportId);
                    if (report == null)
                    {
                        throw new Exception("Báo cáo không tồn tại.");
                    }

                    if (report.Status != 0)
                    {
                        throw new Exception("Báo cáo đã được xử lý, không thể khóa tài khoản.");
                    }

                    report.Status = 1;
                    context.Entry(report).State = EntityState.Modified;

                    int? roomId = report.RoomId;
                    if (!roomId.HasValue)
                    {
                        throw new Exception("Báo cáo không có RoomId liên kết.");
                    }

                    var room = await context.Rooms
                        .FirstOrDefaultAsync(r => r.RoomId == roomId.Value);
                    if (room == null)
                    {
                        throw new Exception("Phòng không tồn tại.");
                    }

                    var landlord = await context.Users
                        .FirstOrDefaultAsync(u => u.UserId == room.UserId);
                    if (landlord == null)
                    {
                        throw new Exception("Chủ phòng không tồn tại.");
                    }
                    landlord.RoleUser = 0;
                    context.Entry(landlord).State = EntityState.Modified;

                    // Gửi thông báo cho người báo cáo
                    var userNotification = new Notification
                    {
                        UserId = report.UserId,
                        Type = "AccountLocked",
                        Message = "Chủ của phòng bạn báo cáo đã bị khóa",
                        RedirectUrl = "/",
                        CreatedDate = DateTime.Now,
                        IsRead = false
                    };

                    await context.Notifications.AddAsync(userNotification);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi khóa tài khoản: {ex.Message}");
            }
        }
    }
}


