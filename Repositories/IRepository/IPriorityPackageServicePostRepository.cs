using DTO;
using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IPriorityPackageServicePostRepository
    {
        Task<List<PriorityPackageServicePostDTO>> GetPriorityPackageServicePostsAsync();
        Task<PriorityPackageServicePostDTO> FindPriorityPackageServicePostByIdAsync(int id);
        Task SavePriorityPackageServicePostAsync(PriorityPackageServicePost package);
        Task UpdatePriorityPackageServicePostAsync(PriorityPackageServicePost package);
        Task DeletePriorityPackageServicePostAsync(int id);
    }
}
