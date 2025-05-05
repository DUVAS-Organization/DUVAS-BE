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
        Task<List<RoomDTO>> GetRoomsByLandlordAsync(int landlordId); // Phương thức lọc theo Landlord
        Task<RoomDTO> GetRoomByIdForLandlordAsync(int roomId, int landlordId); // Lấy phòng theo quyền Landlord
        Task<List<UserFeedbackDTO>> GetRoomReviewsAsync(int roomId); // Lấy đánh giá phòng
        Task<bool> CheckBuildingExistsAsync(int buildingId);
        Task<bool> CheckCategoryExistsAsync(int categoryRoomId);
        Task<bool> CheckUserExistsAsync(int userId); // Kiểm tra UserId có tồn tại không
        Task<List<RoomDTO>> GetRoomsByStatusAsync(int landlordId, int status); // lay phong theo status
        Task<bool> UpdateRoomStatusAsync(int roomId, int landlordId, int status); // luu trang thai phong cua chuc nang manage room status
        Task<List<RoomDTO>> GetAllRoomsByStatusAsync(int status);
        Task<List<RoomDTO>> GetRoomReputationAsync();
        Task<List<RoomDTO>> GetListRoomLockAsync();
        Task<List<RoomDTO>> GetListRoomActiveAsync();
        Task<RoomDTO> GetRoomContractByIdAsync(int roomId);
        Task<List<RoomDTO>> GetRoomRegisterReputationAsync();
        Task LockRoomAsync(int roomId);
        Task UnLockRoomAsync(int roomId);
        Task AcceptReputationAsync(int roomId);
        Task CancelReputationAsync(int roomId);
        Task<Room?> GetRoomEntityByIdForLandlordAsync(int roomId, int landlordId);
        Task<Room?> GetRoomEntityByIdAsync(int roomId);
        Task<bool> CheckRoomIsDuplicatedAsync(int userId, string title, string description);
        Task<bool> CheckDescriptionExistsAsync(string description);
        Task<bool> CheckLocationExistsAsync(string locationDetail);
        Task UpdateAuthorizationAsync(int roomId, int authorization);
        Task<List<RoomDTO>> GetRoomAuthorizationAsync(int? userId);
        Task<Dictionary<int, List<int>>> GetRoomIdsGroupedByBuildingAsync(int userId);
        Task<List<RoomDTO>> SearchRoomsByTermAsync(string searchTerm);


    }
}