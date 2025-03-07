using DTO;
using BusinessObject;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class PriorityPackageServicePostRepository : IPriorityPackageServicePostRepository
    {
        public async Task<List<PriorityPackageServicePostDTO>> GetPriorityPackageServicePostsAsync()
        {
            return await PriorityPackageServicePostDAO.GetPriorityPackageServicePostsAsync();
        }

        public async Task<PriorityPackageServicePostDTO> FindPriorityPackageServicePostByIdAsync(int id)
        {
            return await PriorityPackageServicePostDAO.FindPriorityPackageServicePostByIdAsync(id);
        }

        public async Task SavePriorityPackageServicePostAsync(PriorityPackageServicePost package)
        {
            await PriorityPackageServicePostDAO.SavePriorityPackageServicePostAsync(package);
        }

        public async Task UpdatePriorityPackageServicePostAsync(PriorityPackageServicePost package)
        {
            await PriorityPackageServicePostDAO.UpdatePriorityPackageServicePostAsync(package);
        }

        public async Task DeletePriorityPackageServicePostAsync(int id)
        {
            await PriorityPackageServicePostDAO.DeletePriorityPackageServicePostAsync(id);
        }
    }
}