using DTO;
using DUVAS;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers.Landlord
{
    [Route("api/landlord/[controller]")]
    [ApiController]
    public class RoomManagementController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        public RoomManagementController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        private int GetLandlordId()
        {
            // Lấy UserId từ Claims nếu bạn sử dụng JWT hoặc bất kỳ xác thực nào.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var landlordId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            // Ghi log giá trị để kiểm tra
            Console.WriteLine($"LandlordId: {landlordId}");

            return landlordId;
        }


        // GET: api/landlord/RoomManagement
        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            int landlordId = GetLandlordId();
            var rooms = await _roomRepository.GetRoomsByLandlordAsync(landlordId);
            return Ok(rooms);
        }

        // GET: api/landlord/RoomManagement/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            int landlordId = GetLandlordId();
            var room = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId);
            if (room == null)
            {
                return NotFound("Room not found or access denied.");
            }
            return Ok(room);
        }

        // POST: api/landlord/RoomManagement
        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] RoomDTO roomDto)
        {
            int landlordId = GetLandlordId();

            // Check if UserId exists
            var userExists = await _roomRepository.CheckUserExistsAsync(landlordId);
            if (!userExists)
            {
                return BadRequest("UserId không tồn tại.");
            }

            // Check if BuildingId exists
            var buildingExists = await _roomRepository.CheckBuildingExistsAsync(roomDto.BuildingId);
            if (!buildingExists)
            {
                return BadRequest("BuildingId không tồn tại.");
            }

            // Check if CategoryRoomId exists
            var categoryExists = await _roomRepository.CheckCategoryExistsAsync(roomDto.CategoryRoomId);
            if (!categoryExists)
            {
                return BadRequest("CategoryRoomId không tồn tại.");
            }

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
                IsPermission = roomDto.IsPermission,
                UserId = landlordId,
                BuildingId = roomDto.BuildingId,
                CategoryRoomId = roomDto.CategoryRoomId
            };

            await _roomRepository.SaveRoomAsync(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, room);
        }



        // PUT: api/landlord/RoomManagement/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDTO roomDto)
        {
            int landlordId = GetLandlordId();
            var existingRoom = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId);
            if (existingRoom == null)
            {
                return NotFound("Room not found or access denied.");
            }

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
                Image = roomDto.Image,
                Note = roomDto.Note,
                IsPermission = roomDto.IsPermission,
                UserId = landlordId
            };

            await _roomRepository.UpdateRoomAsync(room);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            int landlordId = GetLandlordId();
            var room = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId); // Lấy đối tượng Room
            if (room == null)
            {
                return NotFound("Room not found or access denied.");
            }

            // Truyền đối tượng Room vào phương thức xóa
            await _roomRepository.DeleteRoomAsync(new Room { RoomId = id });
            return NoContent();
        }



        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> ManageRoomStatus(int id, [FromBody] bool isPermission)
        {
            int landlordId = GetLandlordId();
            var roomDto = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId);
            if (roomDto == null)
            {
                return NotFound("Room not found or access denied.");
            }

            // Chuyển đổi từ RoomDTO sang Room
            var room = new Room
            {
                RoomId = roomDto.RoomId,
                Title = roomDto.Title,
                Description = roomDto.Description,
                LocationDetail = roomDto.LocationDetail,
                Acreage = roomDto.Acreage,
                Furniture = roomDto.Furniture,
                NumberOfBathroom = roomDto.NumberOfBathroom,
                NumberOfBedroom = roomDto.NumberOfBedroom,
                Garret = roomDto.Garret,
                Price = roomDto.Price,
                CategoryRoomId = roomDto.CategoryRoomId,
                Image = roomDto.Image,
                Note = roomDto.Note,
                IsPermission = isPermission,
                UserId = landlordId
            };

            await _roomRepository.UpdateRoomAsync(room);
            return NoContent();
        }


        // GET: api/landlord/RoomManagement/{id}/Reviews
        [HttpGet("{id}/Reviews")]
        public async Task<IActionResult> GetRoomReviews(int id)
        {
            int landlordId = GetLandlordId();
            var room = await _roomRepository.GetRoomByIdForLandlordAsync(id, landlordId);
            if (room == null)
            {
                return NotFound("Room not found or access denied.");
            }

            var reviews = await _roomRepository.GetRoomReviewsAsync(id);
            return Ok(reviews);
        }
    }
}
