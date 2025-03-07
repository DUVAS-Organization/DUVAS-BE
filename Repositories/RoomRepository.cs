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
    public class RoomRepository : IRoomRepository
    {
        public async Task DeleteRoomAsync(Room b) => await RoomDAO.DeleteRoomAsync(b);
        public async Task<Room> GetRoomByIdAsync(int id) => await RoomDAO.FindRoomByIdAsync(id);
        public async Task<List<RoomDTO>> GetRoomsAsync() => await RoomDAO.GetRoomsAsync();
        public async Task SaveRoomAsync(Room b) => await RoomDAO.SaveRoomAsync(b);
        public async Task UpdateRoomAsync(Room b) => await RoomDAO.UpdateRoomAsync(b);
        public async Task<List<RoomDTO>> SearchRoomsAsync(string searchTerm) => await RoomDAO.SearchRoomsAsync(searchTerm);
    }
}
