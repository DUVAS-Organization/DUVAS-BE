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
    public class UserFeedbackDAO
    {
        private readonly ApplicationDbContext _context;

        public UserFeedbackDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<UserFeedbackDTO>> GetUserFeedbacksAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var userFeedbacks = await context.UserFeedbacks
                        .AsNoTracking()
                        .Select(p => new UserFeedbackDTO
                        {
                            UserFeedbackId = p.UserFeedbackId,
                            UserId = p.UserId,
                            Comment = p.Comment,
                            Star = p.Star,
                            Image = p.Image,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return userFeedbacks;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<UserFeedback> FindUserFeedbackByIdAsync(int userFeedbackId)
        {
            UserFeedback userFeedback = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    userFeedback = await context.UserFeedbacks.SingleOrDefaultAsync(x => x.UserFeedbackId == userFeedbackId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return userFeedback;
        }

        public static async Task SaveUserFeedbackAsync(UserFeedback userFeedback)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.UserFeedbacks.AddAsync(userFeedback);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateUserFeedbackAsync(UserFeedback userFeedback)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(userFeedback).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteUserFeedbackAsync(UserFeedback userFeedback)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingUserFeedback = await context.UserFeedbacks.SingleOrDefaultAsync(c => c.UserFeedbackId == userFeedback.UserFeedbackId);
                    if (existingUserFeedback != null)
                    {
                        context.UserFeedbacks.Remove(existingUserFeedback);
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
