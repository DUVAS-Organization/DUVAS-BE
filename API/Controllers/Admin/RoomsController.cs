using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DUVAS;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Repositories.IRepository;
using DTO;

namespace API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ODataController
    {
        private readonly IRoomRepository _roomRepository;

        public RoomsController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // GET: odata/Rooms
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(string searchTerm = null)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                return Ok(await _roomRepository.GetRoomsAsync());
            }

            var rooms = await _roomRepository.SearchRoomsAsync(searchTerm);
            return Ok(rooms);
        }
        [HttpGet("room-locked")]
        public async Task<IActionResult> GetListRoomLock()
        {
            try
            {
                var lockedRooms = await _roomRepository.GetListRoomLockAsync();
                return Ok(lockedRooms ?? new List<RoomDTO>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách phòng bị khóa: {ex.Message}");
            }
        }
        [HttpGet("room-active")]
        public async Task<IActionResult> GetListRoomActiveAsync()
        {
            try
            {
                var activeRooms = await _roomRepository.GetListRoomActiveAsync();
                if (activeRooms == null || activeRooms.Count == 0)
                {
                    return NotFound("Không có phòng nào đang bị khóa.");
                }

                return Ok(activeRooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách phòng active: {ex.Message}");
            }
        }
        // GET: odata/Rooms/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return BadRequest();
            }

            return Ok(room);
        }

        // POST: odata/Rooms
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom([FromBody] Room room)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Thiết lập giá trị mặc định
                room.IsPermission = 1; //Trạng thái bình thường không bị khóa
                room.status = 1; //Trạng thái bình thường(còn trống)
                room.reputation = 0; //Không có tích xanh

                // Lưu phòng vào cơ sở dữ liệu
                await _roomRepository.SaveRoomAsync(room);

                return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, room);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error in PostRoom: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        // PUT: odata/Rooms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, [FromBody] Room room)
        {
            if (id != room.RoomId)
            {
                return BadRequest();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _roomRepository.UpdateRoomAsync(room);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RoomExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: odata/Rooms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return BadRequest();
            }

            await _roomRepository.DeleteRoomAsync(room);
            return NoContent();
        }
        private async Task<bool> RoomExists(int id)
        {
            var Room = await _roomRepository.GetRoomByIdAsync(id);
            return Room != null;
        }
        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockRoom(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room không tồn tại.");
            }

            try
            {
                await _roomRepository.LockRoomAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa room: {ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnLockRoom(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room không tồn tại.");
            }

            try
            {
                await _roomRepository.UnLockRoomAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa room: {ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("acceptReputation/{id}")]
        public async Task<IActionResult> AcceptReputationAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room không tồn tại.");
            }

            try
            {
                await _roomRepository.AcceptReputationAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xác nhận tích xanh: {ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("cancelReputation/{id}")]
        public async Task<IActionResult> CancelReputationAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room không tồn tại.");
            }

            try
            {
                await _roomRepository.CancelReputationAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi hủy tích xanh: {ex.Message}");
            }

            return NoContent();
        }
        [HttpGet("register-reputation")]
        public async Task<IActionResult> GetRoomRegisterReputationAsync()
        {
            var rooms = await _roomRepository.GetRoomRegisterReputationAsync();
            if (rooms == null || rooms.Count == 0)
            {
                return NotFound("Không có phòng nào đăng ký uy tín.");
            }

            return Ok(rooms);
        }
        [HttpGet("{id}/contract")]
        public async Task<IActionResult> GetRoomContract(int id)
        {

            var roomContract = await _roomRepository.GetRoomContractByIdAsync(id);
            if (roomContract == null)
            {
                return NotFound("Thông tin hợp đồng phòng không được tìm thấy.");
            }

            return Ok(roomContract);
        }

    }
}
