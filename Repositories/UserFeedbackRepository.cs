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
    public class UserFeedbackRepository : IUserFeedbackRepository
    {
        public async Task DeleteUserFeedbackAsync(UserFeedback b) => await UserFeedbackDAO.DeleteUserFeedbackAsync(b);
        public async Task<UserFeedback> GetUserFeedbackByIdAsync(int id) => await UserFeedbackDAO.FindUserFeedbackByIdAsync(id);
        public async Task<List<UserFeedbackDTO>> GetUserFeedbacksAsync() => await UserFeedbackDAO.GetUserFeedbacksAsync();
        public async Task SaveUserFeedbackAsync(UserFeedback b) => await UserFeedbackDAO.SaveUserFeedbackAsync(b);
        public async Task UpdateUserFeedbackAsync(UserFeedback b) => await UserFeedbackDAO.UpdateUserFeedbackAsync(b);
    }
}
