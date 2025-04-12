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
    public class CategoryServiceRepository : ICategoryServiceRepository
    {
        public async Task DeleteCategoryServiceAsync(CategoryService b) => await CategoryServiceDAO.DeleteCategoryServiceAsync(b);
        public async Task<CategoryService> GetCategoryServiceByIdAsync(int id) => await CategoryServiceDAO.FindCategoryServiceByIdAsync(id);
        public async Task<List<CategoryServiceDTO>> GetCategoryServicesAsync() => await CategoryServiceDAO.GetCategoryServicesAsync();
        public async Task<List<CategoryServiceDTO>> GetCategoryLockedServicesAsync() => await CategoryServiceDAO.GetCategoryLockedServicesAsync();
        public async Task<List<CategoryServiceDTO>> GetCategoryActiveServicesAsync() => await CategoryServiceDAO.GetCategoryActiveServicesAsync();
        public async Task SaveCategoryServiceAsync(CategoryService b) => await CategoryServiceDAO.SaveCategoryServiceAsync(b);
        public async Task UpdateCategoryServiceAsync(CategoryService b) => await CategoryServiceDAO.UpdateCategoryServiceAsync(b);
        public async Task LockCategoryService(int categoryServiceId) => await CategoryServiceDAO.LockCategoryService(categoryServiceId);
        public async Task UnLockCategoryService(int categoryServiceId) => await CategoryServiceDAO.UnLockCategoryService(categoryServiceId);
    }
}
