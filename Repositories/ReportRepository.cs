using DataAccess;
using DTO;
using DUVAS;
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
       
    }
}
