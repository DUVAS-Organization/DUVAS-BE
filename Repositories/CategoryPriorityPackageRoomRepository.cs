using BusinessObject;
using DataAccess;
using DTO;
using DUVAS;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryPriorityPackageRoomRepository : ICategoryPriorityPackageRoomRepository
    {
        public async Task<List<CategoryPriorityPackageRoomDTO>> GetCategoryPriorityPackageRoomsAsync()
            => await CategoryPriorityPackageRoomDAO.GetCategoryPriorityPackageRoomsAsync();

        public async Task<CategoryPriorityPackageRoom> GetCategoryPriorityPackageRoomByIdAsync(int id)
            => await CategoryPriorityPackageRoomDAO.FindCategoryPriorityPackageRoomByIdAsync(id);

        public async Task SaveCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom)
            => await CategoryPriorityPackageRoomDAO.SaveCategoryPriorityPackageRoomAsync(categoryPriorityPackageRoom);

        public async Task UpdateCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom)
            => await CategoryPriorityPackageRoomDAO.UpdateCategoryPriorityPackageRoomAsync(categoryPriorityPackageRoom);

        public async Task DeleteCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom)
            => await CategoryPriorityPackageRoomDAO.DeleteCategoryPriorityPackageRoomAsync(categoryPriorityPackageRoom);
    }
}