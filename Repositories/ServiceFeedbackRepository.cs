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
    public class ServiceFeedbackRepository : IServiceFeedbackRepository
    {
        public async Task DeleteServiceFeedbackAsync(ServiceFeedback b) => await ServiceFeedbackDAO.DeleteServiceFeedbackAsync(b);
        public async Task<ServiceFeedback> GetServiceFeedbackByIdAsync(int id) => await ServiceFeedbackDAO.FindServiceFeedbackByIdAsync(id);
        public async Task<List<ServiceFeedbackDTO>> GetServiceFeedbacksAsync() => await ServiceFeedbackDAO.GetServiceFeedbacksAsync();
        public async Task SaveServiceFeedbackAsync(ServiceFeedback b) => await ServiceFeedbackDAO.SaveServiceFeedbackAsync(b);
        public async Task UpdateServiceFeedbackAsync(ServiceFeedback b) => await ServiceFeedbackDAO.UpdateServiceFeedbackAsync(b);
    }
}
