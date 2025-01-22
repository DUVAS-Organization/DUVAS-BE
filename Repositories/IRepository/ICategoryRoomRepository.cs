using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICategoryRoomRepository
    {
        Task SaveCategoryRoomAsync(CategoryRoom b);
        Task<CategoryRoom> GetCategoryRoomByIdAsync(int id);
        Task DeleteCategoryRoomAsync(CategoryRoom b);
        Task UpdateCategoryRoomAsync(CategoryRoom b);
        Task<List<CategoryRoomDTO>> GetCategoryRoomsAsync();
    }
}
