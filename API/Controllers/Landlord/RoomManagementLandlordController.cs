using API.Service;
using DataAccess;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Net;
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
        private readonly AiService _aiService;
        private readonly SpeechToTextService _speechToTextService;
        public RoomManagementLandlordController(IRoomRepository roomRepository, 
            UserDAO userDAO
            , CloudinaryService cloudinaryService,
            AiService aiService,
            SpeechToTextService speechToTextService)
        {
            _roomRepository = roomRepository;
            _userDAO = userDAO;
            _cloudinaryService = cloudinaryService; // Inject service upload ảnh
            _aiService = aiService;
            _speechToTextService = speechToTextService;
        }

        [HttpPost("search-rooms")]
        public async Task<IActionResult> SearchRoomsByText([FromQuery] string searchTerm = null)
        {
            Console.WriteLine($"Received searchTerm: '{searchTerm}'");

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("searchTerm is null or empty, returning all rooms.");
                var allRooms = await _roomRepository.GetRoomsAsync();
                return Ok(allRooms);
            }

            var rooms = await _roomRepository.SearchRoomsByTermAsync(searchTerm);

            if (rooms == null || !rooms.Any())
            {
                return Ok(new { message = "No rooms found matching your search." });
            }
            return Ok(new { message = "Rooms found.", rooms });
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
            // ✅ Kiểm tra trùng lặp theo Title + LocationDetail + UserId
            bool isDuplicate = await _roomRepository.CheckRoomIsDuplicatedAsync(
                landlordId,
                roomDto.Title.Trim(),
                //roomDto.LocationDetail.Trim(),
                roomDto.Description.Trim() // Kiểm tra cả mô tả
            );

            if (isDuplicate)
            {
                return Conflict(new
                {
                    message = "Tiêu đề và Mô tả này đã được đăng. Vui lòng kiểm tra lại để tránh trùng lặp."
                });
            }


            // ✅ Kiểm tra Description đã từng được dùng bởi user khác (spam xuyên tài khoản)
            bool isDescUsedGlobally = await _roomRepository.CheckDescriptionExistsAsync(roomDto.Description.Trim());
            if (isDescUsedGlobally)
            {
                return Conflict(new
                {
                    message = "Mô tả phòng đã từng được sử dụng trên hệ thống. Vui lòng điều chỉnh lại nội dung."
                });
            }
            // ✅ Check locationDetail trùng toàn hệ thống
            //bool isLocationUsedGlobally = await _roomRepository.CheckLocationExistsAsync(roomDto.LocationDetail.Trim());
            //if (isLocationUsedGlobally)
            //{
            //    return Conflict(new
            //    {
            //        message = "Địa chỉ phòng đã từng được sử dụng trên hệ thống. Vui lòng kiểm tra lại."
            //    });
            //}


            // ✅ Sử dụng AI để phát hiện nội dung mô tả phòng có bị spam hoặc lặp
            try
            {
                var (generatedTitle, generatedDescription) = await _aiService.GenerateRoomTitleAndDescription(
                    $"Tiêu đề: {roomDto.Title}, Mô tả: {roomDto.Description}, Diện tích: {roomDto.Acreage}, Giá: {roomDto.Price}, Nội thất: {roomDto.Furniture}"
                );

                if (generatedDescription != null && generatedDescription.Equals(roomDto.Description))
                {
                    return BadRequest("Mô tả phòng có dấu hiệu spam hoặc trùng với mô tả đã được tạo tự động. Vui lòng chỉnh sửa lại mô tả.");
                }
            }
            catch (Exception aiEx)
            {
                Console.WriteLine($"[AI CHECK] Lỗi khi kiểm tra AI: {aiEx.Message}");
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
                    Image = roomDto.Image,
                    Note = roomDto.Note,
                    IsPermission = roomDto.IsPermission ?? 1,
                    UserId = landlordId,
                    BuildingId = roomDto.BuildingId,
                    CategoryRoomId = roomDto.CategoryRoomId,
                    status = roomDto.status ?? 1, //Còn trống
                    Deposit = roomDto.Deposit,
                    Garret = roomDto.Garret,
                    reputation = roomDto.reputation ?? 0, //không tích xanh
                    Dien = roomDto.Dien,
                    Nuoc = roomDto.Nuoc,
                    Internet = roomDto.Internet,
                    Rac = roomDto.Rac,
                    GuiXe = roomDto.GuiXe,
                    QuanLy = roomDto.QuanLy,
                    ChiPhiKhac = roomDto.ChiPhiKhac,
                    Authorization = roomDto.Authorization ?? 0,
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
        //[Authorize]
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
                    reputation = roomDto.reputation.HasValue ? roomDto.reputation.Value : existingRoom.reputation,  // Nếu reputation không được truyền, giữ giá trị cũ
                    UserId = landlordId,
                    BuildingId = roomDto.BuildingId,
                    CategoryRoomId = roomDto.CategoryRoomId,
                    Deposit = roomDto.Deposit,
                    Garret = roomDto.Garret,
                    Dien = roomDto.Dien,
                    Nuoc = roomDto.Nuoc,
                    Internet = roomDto.Internet,
                    Rac = roomDto.Rac,
                    GuiXe = roomDto.GuiXe,
                    QuanLy = roomDto.QuanLy,
                    ChiPhiKhac = roomDto.ChiPhiKhac,
                };

                await _roomRepository.UpdateRoomAsync(room);
                return Ok("Bạn đã cập nhật phòng thành công.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi cập nhật phòng.");
            }
        }



        // PATCH: api/landlord/RoomManagement/{id}/lock
        [HttpPatch("{id}/lock")]
        [Authorize]
        public async Task<IActionResult> LockRoom(int id)
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            // Lấy entity Room thật sự (không dùng DTO)
            var room = await _roomRepository.GetRoomEntityByIdForLandlordAsync(id, landlordId);
            if (room == null)
            {
                return NotFound("Không tìm thấy phòng hoặc bạn không có quyền thao tác.");
            }

            try
            {
                room.IsPermission = 0; // ⚠️ Lock phòng lại
                await _roomRepository.UpdateRoomAsync(room);

                return Ok(new { message = "Phòng đã được khóa thành công (IsPermission = 0)." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi khi khóa phòng.", error = ex.Message });
            }
        }
        // GET: api/landlord/RoomManagement/{id}/is-locked
        [HttpGet("{id}/is-locked")]
        [Authorize]
        public async Task<IActionResult> IsRoomLocked(int id)
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            var room = await _roomRepository.GetRoomEntityByIdForLandlordAsync(id, landlordId);
            if (room == null)
            {
                return NotFound("Không tìm thấy phòng hoặc bạn không có quyền truy cập.");
            }

            bool isLocked = room.IsPermission == 0;

            return Ok(new
            {
                RoomId = room.RoomId,
                IsLocked = isLocked,
                Message = isLocked ? "Phòng hiện đang bị khóa." : "Phòng đang hoạt động bình thường."
            });
        }
        // PATCH: api/landlord/RoomManagement/{id}/unlock
        [HttpPatch("{id}/unlock")]
        [Authorize]
        public async Task<IActionResult> UnlockRoom(int id)
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không phải Role Landlord nên không được sử dụng chức năng này.");
            }

            var room = await _roomRepository.GetRoomEntityByIdForLandlordAsync(id, landlordId);
            if (room == null)
            {
                return NotFound("Không tìm thấy phòng hoặc bạn không có quyền thao tác.");
            }

            try
            {
                room.IsPermission = 1; // ✅ Mở khóa phòng
                await _roomRepository.UpdateRoomAsync(room);

                return Ok(new { message = "Phòng đã được mở khóa thành công (IsPermission = 1)." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi khi mở khóa phòng.", error = ex.Message });
            }
        }
        // GET: api/landlord/RoomManagement/locked-rooms
        [HttpGet("locked-rooms")]
        [Authorize]
        public async Task<IActionResult> GetLockedRooms()
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized("Bạn không có quyền truy cập chức năng này.");
            }

            var rooms = await _roomRepository.GetRoomsByLandlordAsync(landlordId);

            var lockedRooms = rooms
                .Where(r => r.IsPermission.HasValue && r.IsPermission == 0)
                .ToList();

            if (!lockedRooms.Any())
            {
                return NotFound("Hiện không có phòng nào đang bị khóa.");
            }

            return Ok(new
            {
                message = "Danh sách các phòng đang bị khóa.",
                rooms = lockedRooms
            });
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
        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateRoomDescription([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Dữ liệu phòng không được để trống.");
            }

            var roomInfo = $"Diện tích: {roomDto.Acreage} m², Nội thất: {roomDto.Furniture}, " +
                           $"Số phòng ngủ: {roomDto.NumberOfBedroom}, Số phòng tắm: {roomDto.NumberOfBathroom}, " +
                           $"Giá: {roomDto.Price} VND, Ghi chú: {roomDto.Note}";

            Console.WriteLine($"[LOG] Dữ liệu gửi đi: {roomInfo}");

            try
            {
                // Gọi phương thức sử dụng Mistral AI thay vì OpenAI
                (string title, string description) = await _aiService.GenerateRoomTitleAndDescription(roomInfo);
                Console.WriteLine($"[LOG] Nhận được kết quả từ AI: Tiêu đề - {title}, Mô tả - {description}");
                return Ok(new { title, description });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[LỖI] Lỗi kết nối Mistral AI: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi kết nối Mistral AI", error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LỖI] Lỗi khi tạo tiêu đề/mô tả: {ex.Message}");
                return BadRequest(new { message = "Lỗi khi tạo tiêu đề và mô tả", error = ex.Message });
            }
        }
    }
}