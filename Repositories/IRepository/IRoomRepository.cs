using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IRoomRepository
    {
        Task SaveRoomAsync(Room b);
        Task<Room> GetRoomByIdAsync(int id);
        Task DeleteRoomAsync(Room b);
        Task UpdateRoomAsync(Room b);
        Task<List<RoomDTO>> GetRoomsAsync();
        Task<List<RoomDTO>> SearchRoomsAsync(string searchTerm);
    }
}
