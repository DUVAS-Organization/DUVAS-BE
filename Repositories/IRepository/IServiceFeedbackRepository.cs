using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IServiceFeedbackRepository
    {
        Task SaveServiceFeedbackAsync(ServiceFeedback b);
        Task<ServiceFeedback> GetServiceFeedbackByIdAsync(int id);
        Task DeleteServiceFeedbackAsync(ServiceFeedback b);
        Task UpdateServiceFeedbackAsync(ServiceFeedback b);
        Task<List<ServiceFeedbackDTO>> GetServiceFeedbacksAsync();
    }
}
