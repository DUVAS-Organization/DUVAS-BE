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
                    var buildings = await context.Reports
                        .AsNoTracking()
                        .Select(p => new ReportDTO
                        {
                            ReportId = p.ReportId,
                            UserId = p.UserId,
                            RoomId = p.RoomId,
                            ServicePostId = p.ServicePostId,
                            TransactionId = p.TransactionId,
                            ReportContent = p.ReportContent,
                            Image = p.Image,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return buildings;
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

    }
}
