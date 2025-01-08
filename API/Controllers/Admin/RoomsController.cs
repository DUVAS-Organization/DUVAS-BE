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

                // Lưu sách vào cơ sở dữ liệu
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

    }
}
