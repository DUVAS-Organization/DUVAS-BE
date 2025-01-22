using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICategoryServiceRepository
    {
        Task SaveCategoryServiceAsync(CategoryService b);
        Task<CategoryService> GetCategoryServiceByIdAsync(int id);
        Task DeleteCategoryServiceAsync(CategoryService b);
        Task UpdateCategoryServiceAsync(CategoryService b);
        Task<List<CategoryServiceDTO>> GetCategoryServicesAsync();
    }
}
