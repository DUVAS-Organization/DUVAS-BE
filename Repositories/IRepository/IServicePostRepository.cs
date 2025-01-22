using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IServicePostRepository
    {
        Task SaveServicePostAsync(ServicePost b);
        Task<ServicePost> GetServicePostByIdAsync(int id);
        Task DeleteServicePostAsync(ServicePost b);
        Task UpdateServicePostAsync(ServicePost b);
        Task<List<ServicePostDTO>> GetServicePostsAsync();
    }
}
