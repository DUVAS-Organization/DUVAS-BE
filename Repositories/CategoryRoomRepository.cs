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
    public class CategoryRoomRepository : ICategoryRoomRepository
    {
        public async Task DeleteCategoryRoomAsync(CategoryRoom b) => await CategoryRoomDAO.DeleteCategoryRoomAsync(b);
        public async Task<CategoryRoom> GetCategoryRoomByIdAsync(int id) => await CategoryRoomDAO.FindCategoryRoomByIdAsync(id);
        public async Task<List<CategoryRoomDTO>> GetCategoryRoomsAsync() => await CategoryRoomDAO.GetCategoryRoomsAsync();
        public async Task SaveCategoryRoomAsync(CategoryRoom b) => await CategoryRoomDAO.SaveCategoryRoomAsync(b);
        public async Task UpdateCategoryRoomAsync(CategoryRoom b) => await CategoryRoomDAO.UpdateCategoryRoomAsync(b);
       
    }
}
