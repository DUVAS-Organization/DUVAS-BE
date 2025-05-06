using DataAccess;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ReportRepository : IReportRepository
    {
        public async Task DeleteReportAsync(Report b) => await ReportDAO.DeleteReportAsync(b);
        public async Task<Report> GetReportByIdAsync(int id) => await ReportDAO.FindReportByIdAsync(id);
        public async Task<List<ReportDTO>> GetReportsAsync() => await ReportDAO.GetReportsAsync();
        public async Task SaveReportAsync(Report b) => await ReportDAO.SaveReportAsync(b);
        public async Task UpdateReportAsync(Report b) => await ReportDAO.UpdateReportAsync(b);
        public async Task<List<ReportDTO>> GetReportsByUserIdAsync(int userId) => await ReportDAO.GetReportsByUserIdAsync(userId);
        public async Task<bool> HasPendingReport(int userId, int roomId)
    => await ReportDAO.HasPendingReport(userId, roomId);
        public async Task<List<ReportDTO>> GetReportsByLandlordIdAsync(int landlordId) => await ReportDAO.GetReportsByLandlordIdAsync(landlordId);
        public async Task<int> GetRoomOwnerIdAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var room = await context.Rooms
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.RoomId == roomId);

                    return room?.UserId ?? 0; // Trả về 0 nếu không tìm thấy
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
