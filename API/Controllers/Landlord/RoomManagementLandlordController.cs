using API.Service;
using DataAccess;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers.Landlord
{
    [Route("api/landlord/[controller]")]
    [ApiController]
    public class RoomManagementLandlordController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserDAO _userDAO;
        private readonly CloudinaryService _cloudinaryService;

        public RoomManagementLandlordController(IRoomRepository roomRepository, UserDAO userDAO, CloudinaryService cloudinaryService)
        {
            _roomRepository = roomRepository;
            _userDAO = userDAO;
            _cloudinaryService = cloudinaryService; // Inject service upload ảnh
        }


        private int GetLandlordId()
        {
            var userIdClaim = User.FindFirst("UserId"); // Lấy claim "UserId" thay vì NameIdentifier
            var landlordId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
            Console.WriteLine($"LandlordId: {landlordId}");
            return landlordId;
        }

        private async Task<bool> IsLandlord(int userId)
        {
            var user = await UserDAO.FindUserByIdAsync(userId);
            Console.WriteLine($"UserId: {userId}, RoleLandlord: {user?.RoleLandlord}");
            return user?.RoleLandlord == 1;
        }


        // GET: api/landlord/RoomManagement
        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            int landlordId = GetLandlordId();

            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này");
            }

            var rooms = await _roomRepository.GetRoomsByLandlordAsync(landlordId);

            if (rooms == null || rooms.Count == 0)
            {
                return NotFound("Bạn hiện không có phòng nào trong hệ thống.");
            }

            return Ok(new { Message = "Danh sách tất cả phòng của bạn.", Rooms = rooms });
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRoomsByStatus([FromQuery] int status)
        {
            int landlordId = GetLandlordId();

            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không có quyền truy cập chức năng này.");
            }

            var rooms = await _roomRepository.GetRoomsByStatusAsync(landlordId, status);

            if (rooms == null || rooms.Count == 0)
            {
                string noRoomMessage = status switch
                {
                    1 => "Hiện không có phòng nào đang trống.",
                    2 => "Hiện không có phòng nào đang ở trạng thái pending.",
                    3 => "Hiện không có phòng nào đang được thuê.",
                    _ => "Không có phòng nào phù hợp với trạng thái này."
                };
                return NotFound(noRoomMessage);
            }

            string successMessage = status switch
            {
                1 => "Đây là tất cả phòng trống.",
                2 => "Đây là tất cả phòng đang chờ xác nhận (pending).",
                3 => "Đây là tất cả phòng đang được thuê.",
                _ => "Danh sách phòng theo trạng thái."
            };

            return Ok(new { message = successMessage, rooms });
        }



        // GET: api/landlord/RoomManagement/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            int landlordId = GetLandlordId();
            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            var room = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId);
            if (room == null)
            {
                return NotFound("Room not found or access denied.");
            }
            return Ok(room);
        }

        // POST: api/landlord/RoomManagement
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRoom([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Dữ liệu phòng không được để trống.");
            }

            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            // Kiểm tra UserId có tồn tại không
            var userExists = await _roomRepository.CheckUserExistsAsync(landlordId);
            if (!userExists)
            {
                return BadRequest("UserId không tồn tại.");
            }

            // Kiểm tra BuildingId có tồn tại không
            if (roomDto.BuildingId.HasValue && !await _roomRepository.CheckBuildingExistsAsync(roomDto.BuildingId.Value))
            {
                return BadRequest("BuildingId không tồn tại.");
            }

            // Kiểm tra CategoryRoomId có tồn tại không
            var categoryExists = await _roomRepository.CheckCategoryExistsAsync(roomDto.CategoryRoomId);
            if (!categoryExists)
            {
                return BadRequest("CategoryRoomId không tồn tại.");
            }

            try
            {
                var room = new Room
                {
                    Title = roomDto.Title,
                    Description = roomDto.Description,
                    LocationDetail = roomDto.LocationDetail,
                    Acreage = roomDto.Acreage,
                    Furniture = roomDto.Furniture,
                    NumberOfBathroom = roomDto.NumberOfBathroom,
                    NumberOfBedroom = roomDto.NumberOfBedroom,
                    Price = roomDto.Price,
                    Image = roomDto.Image,  // Sử dụng URL ảnh trực tiếp
                    Note = roomDto.Note,
                    IsPermission = roomDto.IsPermission,
                    UserId = landlordId,
                    BuildingId = roomDto.BuildingId,
                    CategoryRoomId = roomDto.CategoryRoomId,
                    status = roomDto.status ?? 1,
                    Deposit = roomDto.Deposit,  // Thêm giá trị tiền đặt cọc
                    Garret = roomDto.Garret  // Thêm giá trị có gác mái
                };

                await _roomRepository.SaveRoomAsync(room);

                return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, new { message = "Bạn đã thêm thành công phòng mới", room });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return BadRequest(new { message = "Lỗi khi thêm phòng", error = ex.Message });
            }
        }




        // PUT: api/landlord/RoomManagement/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDTO roomDto)
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            var existingRoom = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId);
            if (existingRoom == null)
            {
                return NotFound("Room không tồn tại hoặc quyền truy cập bị từ chối.");
            }

            // Kiểm tra dữ liệu thiếu thông tin
            if (string.IsNullOrEmpty(roomDto.Title) || string.IsNullOrEmpty(roomDto.Description))
            {
                return BadRequest("Lỗi khi cập nhật phòng: Thiếu thông tin quan trọng (Title, Description).");
            }

            try
            {
                // Cập nhật các trường của phòng
                var room = new Room
                {
                    RoomId = id,
                    Title = roomDto.Title,
                    Description = roomDto.Description,
                    LocationDetail = roomDto.LocationDetail,
                    Acreage = roomDto.Acreage,
                    Furniture = roomDto.Furniture,
                    NumberOfBathroom = roomDto.NumberOfBathroom,
                    NumberOfBedroom = roomDto.NumberOfBedroom,
                    Price = roomDto.Price,
                    Image = roomDto.Image,  // Sử dụng URL ảnh trực tiếp
                    Note = roomDto.Note,
                    IsPermission = roomDto.IsPermission.HasValue ? roomDto.IsPermission.Value : existingRoom.IsPermission,  // Nếu IsPermission không được truyền, giữ giá trị cũ
                    status = roomDto.status.HasValue ? roomDto.status.Value : existingRoom.status,  // Nếu status không được truyền, giữ giá trị cũ
                    UserId = landlordId,
                    BuildingId = roomDto.BuildingId,
                    CategoryRoomId = roomDto.CategoryRoomId,
                    Deposit = roomDto.Deposit,
                    Garret = roomDto.Garret
                };

                await _roomRepository.UpdateRoomAsync(room);
                return Ok("Bạn đã cập nhật phòng thành công.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi cập nhật phòng.");
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            var room = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId); // Lấy đối tượng Room
            if (room == null)
            {
                return NotFound("Room not found or access denied.");
            }

            // Truyền đối tượng Room vào phương thức xóa
            await _roomRepository.DeleteRoomAsync(new Room { RoomId = id });
            return NoContent();
        }

        // GET: api/landlord/RoomManagement/{id}/Reviews
        [HttpGet("{id}/Reviews")]
        public async Task<IActionResult> GetRoomReviews(int id)
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            var room = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId);
            if (room == null)
            {
                return NotFound("Room not found or access denied.");
            }

            var reviews = await _roomRepository.GetRoomReviewsAsync(id);
            return Ok(reviews);
        }

        // PATCH: api/landlord/RoomManagement/{id}/Status
        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> ManageRoomStatus([FromQuery] int roomId, [FromQuery] int status)
        {
            int landlordId = GetLandlordId();

            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không có quyền thực hiện thao tác này.");
            }

            bool isUpdated = await _roomRepository.UpdateRoomStatusAsync(roomId, landlordId, status);
            if (!isUpdated)
            {
                return NotFound("Không tìm thấy phòng hoặc bạn không có quyền chỉnh sửa.");
            }

            return Ok(new { Message = "Trạng thái phòng đã được cập nhật thành công." });
        }
    }
}