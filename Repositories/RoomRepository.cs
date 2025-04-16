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
        public async Task<List<RoomDTO>> GetRoomsByStatusAsync(int landlordId, int status)
        {
            return await RoomDAO.GetRoomsByStatusAsync(landlordId, status);
        }

        public async Task<bool> UpdateRoomStatusAsync(int roomId, int landlordId, int status)
        {
            using var context = new ApplicationDbContext();
            var room = await context.Rooms
                                    .FirstOrDefaultAsync(r => r.RoomId == roomId && r.UserId == landlordId);

            if (room == null) return false;

            room.status = status;  // Gán giá trị trạng thái mới cho phòng
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<RoomDTO>> GetAllRoomsByStatusAsync(int status)
        {
            using var context = new ApplicationDbContext();
            return await RoomDAO.GetAllRoomsByStatusAsync(status);
        }
        public async Task<List<RoomDTO>> GetRoomReputationAsync()
        {
            return await RoomDAO.GetRoomReputationAsync();
        }
        public async Task<List<RoomDTO>> GetListRoomLockAsync()
        {
            return await RoomDAO.GetListRoomLockAsync();
        }
        public async Task<List<RoomDTO>> GetListRoomActiveAsync()
        {
            return await RoomDAO.GetListRoomActiveAsync();
        }
        public async Task LockRoomAsync(int roomId) => await RoomDAO.LockRoomAsync(roomId);
        public async Task UnLockRoomAsync(int roomId) => await RoomDAO.UnLockRoomAsync(roomId);
        public async Task AcceptReputationAsync(int roomId) => await RoomDAO.AcceptReputationAsync(roomId);
        public async Task CancelReputationAsync(int roomId) => await RoomDAO.CancelReputationAsync(roomId);
        public async Task<RoomDTO> GetRoomContractByIdAsync(int roomId)
        {
            return await RoomDAO.GetRoomContractByIdAsync(roomId);
        }
        public async Task<List<RoomDTO>> GetRoomRegisterReputationAsync()
        {
            return await RoomDAO.GetRoomRegisterReputationAsync();
        }
        public async Task<Room?> GetRoomEntityByIdForLandlordAsync(int roomId, int landlordId)
        {
            return await RoomDAO.GetRoomEntityByIdForLandlordAsync(roomId, landlordId);
        }
        public async Task<bool> CheckRoomIsDuplicatedAsync(int userId, string title, string locationDetail, string description)
        {
            return await RoomDAO.CheckRoomIsDuplicatedAsync(userId, title, locationDetail, description);
        }
        public async Task<bool> CheckDescriptionExistsAsync(string description)
        {
            return await RoomDAO.CheckDescriptionExistsAsync(description);
        }
        public async Task<bool> CheckLocationExistsAsync(string locationDetail)
        {
            return await RoomDAO.CheckLocationExistsAsync(locationDetail);
        }
        public async Task<List<RoomDTO>> GetRoomAuthorizationAsync(string? userName = null)
        {
            return await RoomDAO.GetRoomAuthorizationAsync(userName);
        }

    }
}