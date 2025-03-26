using DataAccess;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ServicePostRepository : IServicePostRepository
    {
        public async Task DeleteServicePostAsync(ServicePost b) => await ServicePostDAO.DeleteServicePostAsync(b);
        public async Task<ServicePost> GetServicePostByIdAsync(int id) => await ServicePostDAO.FindServicePostByIdAsync(id);
        public async Task<List<ServicePostDTO>> GetServicePostsAsync() => await ServicePostDAO.GetServicePostsAsync();
        public async Task SaveServicePostAsync(ServicePost b) => await ServicePostDAO.SaveServicePostAsync(b);
        public async Task UpdateServicePostAsync(ServicePost b) => await ServicePostDAO.UpdateServicePostAsync(b);

        public async Task<List<ServicePostDTO>> SearchServicePostsAsync(string searchTerm) => await ServicePostDAO.SearchServicePostsAsync(searchTerm);
        public async Task LockServicePostAsync(int servicepostId) => await ServicePostDAO.LockServicePostAsync(servicepostId);
        public async Task UnLockServicePostAsync(int servicepostId) => await ServicePostDAO.UnLockServicePostAsync(servicepostId);
        public async Task<List<ServicePostDTO>> GetListServicePostLockAsync() => await ServicePostDAO.GetListServicePostLockAsync();

        public async Task<List<ServicePostDTO>> GetListServicePostActiveAsync() => await ServicePostDAO.GetListServicePostActiveAsync();
        public async Task<List<ServicePostDTO>> GetServicePostsByUserIdAsync(int userId)
        {
            return await ServicePostDAO.GetServicePostsByUserIdAsync(userId);
        }


    }
}
