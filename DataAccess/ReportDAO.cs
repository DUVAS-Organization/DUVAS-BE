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

    }
}
