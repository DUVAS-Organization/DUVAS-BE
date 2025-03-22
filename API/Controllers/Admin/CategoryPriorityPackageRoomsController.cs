using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.IRepository;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using BusinessObject;

namespace API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryPriorityPackageRoomsController : ODataController
    {
        private readonly ICategoryPriorityPackageRoomRepository _categoryPriorityPackageRoomRepository;

        public CategoryPriorityPackageRoomsController(ICategoryPriorityPackageRoomRepository categoryPriorityPackageRoomRepository)
        {
            _categoryPriorityPackageRoomRepository = categoryPriorityPackageRoomRepository;
        }

        // GET: odata/CategoryPriorityPackageRooms
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryPriorityPackageRoomDTO>>> GetCategoryPriorityPackageRooms()
        {
            var categoryPriorityPackageRooms = await _categoryPriorityPackageRoomRepository.GetCategoryPriorityPackageRoomsAsync();
            return Ok(categoryPriorityPackageRooms);
        }

        // GET: odata/CategoryPriorityPackageRooms/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryPriorityPackageRoom>> GetCategoryPriorityPackageRoom(int id)
        {
            var categoryPriorityPackageRoom = await _categoryPriorityPackageRoomRepository.GetCategoryPriorityPackageRoomByIdAsync(id);
            if (categoryPriorityPackageRoom == null)
            {
                return NotFound();
            }

            return Ok(categoryPriorityPackageRoom);
        }

        // POST: odata/CategoryPriorityPackageRooms
        [HttpPost]
        public async Task<ActionResult<CategoryPriorityPackageRoom>> PostCategoryPriorityPackageRoom([FromBody] CategoryPriorityPackageRoom categoryPriorityPackageRoom)
        {
            await _categoryPriorityPackageRoomRepository.SaveCategoryPriorityPackageRoomAsync(categoryPriorityPackageRoom);
            return CreatedAtAction(nameof(GetCategoryPriorityPackageRoom), new { id = categoryPriorityPackageRoom.CategoryPriorityPackageRoomId }, categoryPriorityPackageRoom);
        }

        // PUT: odata/CategoryPriorityPackageRooms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryPriorityPackageRoom(int id, [FromBody] CategoryPriorityPackageRoom categoryPriorityPackageRoom)
        {
            if (id != categoryPriorityPackageRoom.CategoryPriorityPackageRoomId)
            {
                return BadRequest();
            }

            try
            {
                await _categoryPriorityPackageRoomRepository.UpdateCategoryPriorityPackageRoomAsync(categoryPriorityPackageRoom);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryPriorityPackageRoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: odata/CategoryPriorityPackageRooms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryPriorityPackageRoom(int id)
        {
            var categoryPriorityPackageRoom = await _categoryPriorityPackageRoomRepository.GetCategoryPriorityPackageRoomByIdAsync(id);
            if (categoryPriorityPackageRoom == null)
            {
                return NotFound();
            }

            await _categoryPriorityPackageRoomRepository.DeleteCategoryPriorityPackageRoomAsync(categoryPriorityPackageRoom);
            return NoContent();
        }

        private async Task<bool> CategoryPriorityPackageRoomExists(int id)
        {
            var categoryPriorityPackageRoom = await _categoryPriorityPackageRoomRepository.GetCategoryPriorityPackageRoomByIdAsync(id);
            return categoryPriorityPackageRoom != null;
        }
    }
}