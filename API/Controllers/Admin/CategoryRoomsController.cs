using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DUVAS;
using Microsoft.AspNetCore.OData.Query;
using Repositories.IRepository;
using DTO;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;

namespace API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryRoomsController : ODataController
    {
        private readonly ICategoryRoomRepository _categoryRoomRepository;

        public CategoryRoomsController(ICategoryRoomRepository categoryRoomRepository)
        {
            _categoryRoomRepository = categoryRoomRepository;
        }

        // GET: odata/Category
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryRoomDTO>>> GetCategoryRooms()
        {
            var categoryRooms = await _categoryRoomRepository.GetCategoryRoomsAsync();
            return Ok(categoryRooms);
        }

        // GET: odata/Category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryRoom>> GetCategoryRoom(int id)
        {
            var categoryRooms = await _categoryRoomRepository.GetCategoryRoomByIdAsync(id);
            if (categoryRooms == null)
            {
                return BadRequest();
            }

            return Ok(categoryRooms);
        }

        // POST: odata/Category
        [HttpPost]
        public async Task<ActionResult<CategoryRoom>> PostCategoryRoom([FromBody] CategoryRoom categoryRooms)
        {
            await _categoryRoomRepository.SaveCategoryRoomAsync(categoryRooms);
            return CreatedAtAction(nameof(GetCategoryRoom), new { id = categoryRooms.CategoryRoomId }, categoryRooms);
        }

        // PUT: odata/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CategoryRoom categoryRooms)
        {
            if (id != categoryRooms.CategoryRoomId)
            {
                return BadRequest();
            }

            try
            {
                await _categoryRoomRepository.UpdateCategoryRoomAsync(categoryRooms);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryRoomExists(id))
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

        // DELETE: odata/Category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryRoom(int id)
        {
            var categoryRooms = await _categoryRoomRepository.GetCategoryRoomByIdAsync(id);
            if (categoryRooms == null)
            {
                return BadRequest();
            }

            await _categoryRoomRepository.DeleteCategoryRoomAsync(categoryRooms);
            return NoContent();
        }
        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockCategoryRoom(int id)
        {
            var categoryRooms = await _categoryRoomRepository.GetCategoryRoomByIdAsync(id);
            if (categoryRooms == null)
            {
                return NotFound("Room không tồn tại.");
            }

            try
            {
                await _categoryRoomRepository.LockCategoryRoom(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa Room: {ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnLockCategoryRoom(int id)
        {
            var categoryRooms = await _categoryRoomRepository.GetCategoryRoomByIdAsync(id);
            if (categoryRooms == null)
            {
                return NotFound("Room không tồn tại.");
            }

            try
            {
                await _categoryRoomRepository.UnLockCategoryRoom(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa Room: {ex.Message}");
            }

            return NoContent();
        }
        private async Task<bool> CategoryRoomExists(int id)
        {
            var categoryRooms = await _categoryRoomRepository.GetCategoryRoomByIdAsync(id);
            return categoryRooms != null;
        }

    }
}
