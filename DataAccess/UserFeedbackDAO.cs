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

        public async Task SaveUserFeedbackAsync(UserFeedbackDTO userFeedback)
        {
            try
            {
                var feedback = new UserFeedback
                {
                    UserId = userFeedback.UserId,
                    Comment = userFeedback.Comment,
                    Star = userFeedback.Star,
                    Image = userFeedback.Image,
                    CreatedDate = DateTime.UtcNow,
                    RoomId = userFeedback.RoomId,
                };
                _context.UserFeedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                // ✅ Gửi thông báo (cho admin hoặc người đăng phòng tuỳ logic)
                var notification = new Notification
                {
                    UserId = userFeedback.UserId, // hoặc ID chủ phòng nếu bạn muốn họ nhận thông báo
                    Type = "UserFeedback",
                    Message = $"Bạn đã gửi đánh giá với {userFeedback.Star} sao.",
                    RedirectUrl = "/feedbacks", // hoặc /rooms/{RoomId} nếu muốn trỏ đến phòng
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                };

                await NotificationDAO.CreateNotificationAsync(notification);

            }
            catch (Exception ex)
            {
                // Log the error if needed
                throw new Exception("Error saving user feedback: " + ex.Message);
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
        public async Task<IEnumerable<object>> GetUserFeedbacksByRoomIdAsync(int roomId)
        {
            try
            {
                var feedbacks = await _context.UserFeedbacks
                    .Where(f => f.RoomId == roomId).Include(userFeedback => userFeedback.User)
                    .ToListAsync();

                return feedbacks.Select(f => new
                {
                    Id = f.UserFeedbackId,
                    UserId = f.UserId,
                    UserName = f.User.UserName,
                    UserAvatar = f.User.ProfilePicture ?? string.Empty,
                    Comment = f.Comment,
                    Image = f.Image,
                    Rating = f.Star,
                    CreatedAt = f.CreatedDate
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}