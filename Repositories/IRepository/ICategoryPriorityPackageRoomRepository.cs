using BusinessObject;
using DTO;
using DUVAS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICategoryPriorityPackageRoomRepository
    {
        Task<List<CategoryPriorityPackageRoomDTO>> GetCategoryPriorityPackageRoomsAsync();
        Task<CategoryPriorityPackageRoom> GetCategoryPriorityPackageRoomByIdAsync(int id);
        Task SaveCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom);
        Task UpdateCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom);
        Task DeleteCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom);
    }
}
