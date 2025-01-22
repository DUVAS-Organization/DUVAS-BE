using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IUserFeedbackRepository
    {
        Task SaveUserFeedbackAsync(UserFeedback b);
        Task<UserFeedback> GetUserFeedbackByIdAsync(int id);
        Task DeleteUserFeedbackAsync(UserFeedback b);
        Task UpdateUserFeedbackAsync(UserFeedback b);
        Task<List<UserFeedbackDTO>> GetUserFeedbacksAsync();
    }
}
