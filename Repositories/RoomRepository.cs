using DataAccess;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<RoomDTO>> GetRoomsByLandlordAsync(int landlordId) => await RoomDAO.GetRoomsByLandlordAsync(landlordId);

        public async Task<RoomDTO> GetRoomByIdForLandlordAsync(int roomId, int landlordId)
        {
            return await RoomDAO.GetRoomByIdForLandlordAsync(roomId, landlordId);
        }

        public async Task<List<UserFeedbackDTO>> GetRoomReviewsAsync(int roomId)
        {
            return await RoomDAO.GetRoomReviewsAsync(roomId);
        }

        public async Task<bool> CheckBuildingExistsAsync(int buildingId)
        {
            using var context = new ApplicationDbContext();
            return await context.Buildings.AnyAsync(b => b.BuildingId == buildingId);
        }

        public async Task<bool> CheckCategoryExistsAsync(int categoryRoomId)
        {
            using var context = new ApplicationDbContext();
            return await context.CategoryRooms.AnyAsync(c => c.CategoryRoomId == categoryRoomId);
        }

        public async Task<bool> CheckUserExistsAsync(int userId)
        {
            using var context = new ApplicationDbContext();
            return await context.Users.AnyAsync(u => u.UserId == userId);
        }
    }
}
