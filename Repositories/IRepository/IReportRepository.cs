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
    }
}
