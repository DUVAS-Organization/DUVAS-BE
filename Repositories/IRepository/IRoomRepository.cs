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
    }
}
