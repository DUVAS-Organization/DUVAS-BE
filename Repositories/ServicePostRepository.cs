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
    public class ServicePostRepository : IServicePostRepository
    {
        public async Task DeleteServicePostAsync(ServicePost b) => await ServicePostDAO.DeleteServicePostAsync(b);
        public async Task<ServicePost> GetServicePostByIdAsync(int id) => await ServicePostDAO.FindServicePostByIdAsync(id);
        public async Task<List<ServicePostDTO>> GetServicePostsAsync() => await ServicePostDAO.GetServicePostsAsync();
        public async Task SaveServicePostAsync(ServicePost b) => await ServicePostDAO.SaveServicePostAsync(b);
        public async Task UpdateServicePostAsync(ServicePost b) => await ServicePostDAO.UpdateServicePostAsync(b);
    }
}
