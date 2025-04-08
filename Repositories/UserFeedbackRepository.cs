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
        private readonly UserFeedbackDAO _userFeedbackDAO;

        public UserFeedbackRepository(UserFeedbackDAO userFeedbackDAO)
        {
            _userFeedbackDAO = userFeedbackDAO;
        }

        public async Task DeleteUserFeedbackAsync(UserFeedback b) => await UserFeedbackDAO.DeleteUserFeedbackAsync(b);
        public async Task<UserFeedback> GetUserFeedbackByIdAsync(int id) => await UserFeedbackDAO.FindUserFeedbackByIdAsync(id);
        public async Task<List<UserFeedbackDTO>> GetUserFeedbacksAsync() => await UserFeedbackDAO.GetUserFeedbacksAsync();
        public async Task SaveUserFeedbackAsync(UserFeedbackDTO feedback) => await _userFeedbackDAO.SaveUserFeedbackAsync(feedback);
        public async Task UpdateUserFeedbackAsync(UserFeedback b) => await UserFeedbackDAO.UpdateUserFeedbackAsync(b);
        public async Task<IEnumerable<object>> GetUserFeedbacksByRoomIdAsync(int roomId) => await _userFeedbackDAO.GetUserFeedbacksByRoomIdAsync(roomId);
    }
}