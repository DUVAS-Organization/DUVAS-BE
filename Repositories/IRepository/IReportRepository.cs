using DataAccess;
using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IReportRepository
    {
        Task SaveReportAsync(Report b);
        Task<Report> GetReportByIdAsync(int id);
        Task DeleteReportAsync(Report b);
        Task UpdateReportAsync(Report b);
        Task<List<ReportDTO>> GetReportsAsync();
        Task<List<ReportDTO>> GetReportsByUserIdAsync(int userId);
        public async Task<List<ReportDTO>> GetPendingReportsByUserAndRoomAsync(int userId, int? roomId) => await ReportDAO.GetPendingReportsByUserAndRoomAsync(userId, roomId);
        Task<bool> HasPendingReport(int userId, int roomId);
        Task<List<ReportDTO>> GetReportsByLandlordIdAsync(int landlordId);
        Task<int> GetRoomOwnerIdAsync(int roomId);
        Task LockRoomAsync(int reportId);
        Task LockAccountAsync(int reportId);
    }
}
