using BusinessObject;
using DataAccess;
using DTO;
using DUVAS;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryPriorityPackageServicePostRepository : ICategoryPriorityPackageServicePostRepository
    {
        public async Task<List<CategoryPriorityPackageServicePostDTO>> GetCategoryPriorityPackageServicePostsAsync()
            => await CategoryPriorityPackageServicePostDAO.GetCategoryPriorityPackageServicePostsAsync();

        public async Task<CategoryPriorityPackageServicePost> GetCategoryPriorityPackageServicePostByIdAsync(int id)
            => await CategoryPriorityPackageServicePostDAO.FindCategoryPriorityPackageServicePostByIdAsync(id);

        public async Task SaveCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
            => await CategoryPriorityPackageServicePostDAO.SaveCategoryPriorityPackageServicePostAsync(categoryPriorityPackageServicePost);

        public async Task UpdateCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
            => await CategoryPriorityPackageServicePostDAO.UpdateCategoryPriorityPackageServicePostAsync(categoryPriorityPackageServicePost);

        public async Task DeleteCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
            => await CategoryPriorityPackageServicePostDAO.DeleteCategoryPriorityPackageServicePostAsync(categoryPriorityPackageServicePost);
    }
}
