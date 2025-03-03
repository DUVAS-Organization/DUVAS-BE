using BusinessObject;
using DTO;
using DUVAS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICategoryPriorityPackageServicePostRepository
    {
        Task<List<CategoryPriorityPackageServicePostDTO>> GetCategoryPriorityPackageServicePostsAsync();
        Task<CategoryPriorityPackageServicePost> GetCategoryPriorityPackageServicePostByIdAsync(int id);
        Task SaveCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost);
        Task UpdateCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost);
        Task DeleteCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost);
    }
}
