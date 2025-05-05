using DTO;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityPackageRoomController : ControllerBase
    {
        private readonly IPriorityPackageRoomRepository _repository;

        public PriorityPackageRoomController(IPriorityPackageRoomRepository repository)
        {
            _repository = repository;
        }

        // Lấy danh sách tất cả PriorityPackageRoom
        [HttpGet]
        public async Task<ActionResult<List<PriorityPackageRoomDTO>>> GetAll()
        {
            try
            {
                var rooms = await _repository.GetPriorityPackageRoomsAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Lấy PriorityPackageRoom theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PriorityPackageRoomDTO>> GetById(int id)
        {
            try
            {
                var room = await _repository.FindPriorityPackageRoomByIdAsync(id);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Lấy PriorityPackageRoom theo ID
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PriorityPackageRoomDTO>> GetByUserId(int userId)
        {
            try
            {
                var room = await _repository.GetPriorityPackageRoomByUserIdAsync(userId);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server nội bộ. Vui lòng thử lại sau." });
                return NotFound(new { message = ex.Message });
            }
        }

        // Thêm mới PriorityPackageRoom
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PriorityPackageRoom package)
        {
            try
            {
                await _repository.SavePriorityPackageRoomAsync(package);
                return CreatedAtAction(nameof(GetById), new { id = package.PriorityPackageRoomId }, package);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Cập nhật PriorityPackageRoom
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PriorityPackageRoom package)
        {
            if (id != package.PriorityPackageRoomId)
            {
                return BadRequest(new { message = "ID không khớp." });
            }

            try
            {
                await _repository.UpdatePriorityPackageRoomAsync(package);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Xóa PriorityPackageRoom
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeletePriorityPackageRoomAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}