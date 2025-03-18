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
        Task<List<ServicePostDTO>> SearchServicePostsAsync(string searchTerm);
        Task LockServicePostAsync(int servicepostId);
        Task UnLockServicePostAsync(int servicepostId);
        Task<List<ServicePostDTO>> GetListServicePostLockAsync();
        Task<List<ServicePostDTO>> GetListServicePostActiveAsync();
    }
}
