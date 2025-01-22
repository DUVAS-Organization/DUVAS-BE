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
    public class ServiceFeedbackDAO
    {
        private readonly ApplicationDbContext _context;

        public ServiceFeedbackDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<ServiceFeedbackDTO>> GetServiceFeedbacksAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var serviceFeedbacks = await context.ServiceFeedbacks
                        .AsNoTracking()
                        .Select(p => new ServiceFeedbackDTO
                        {
                            ServiceFeedbackId = p.ServiceFeedbackId,
                            ServicePostId = p.ServicePostId,
                            Comment = p.Comment,
                            Star = p.Star,
                            Image = p.Image,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return serviceFeedbacks;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<ServiceFeedback> FindServiceFeedbackByIdAsync(int serviceFeedbackId)
        {
            ServiceFeedback serviceFeedback = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    serviceFeedback = await context.ServiceFeedbacks.SingleOrDefaultAsync(x => x.ServiceFeedbackId == serviceFeedbackId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return serviceFeedback;
        }

        public static async Task SaveServiceFeedbackAsync(ServiceFeedback serviceFeedback)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.ServiceFeedbacks.AddAsync(serviceFeedback);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateServiceFeedbackAsync(ServiceFeedback serviceFeedback)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(serviceFeedback).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteServiceFeedbackAsync(ServiceFeedback serviceFeedback)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingServiceFeedback = await context.ServiceFeedbacks.SingleOrDefaultAsync(c => c.ServiceFeedbackId == serviceFeedback.ServiceFeedbackId);
                    if (existingServiceFeedback != null)
                    {
                        context.ServiceFeedbacks.Remove(existingServiceFeedback);
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
