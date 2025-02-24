using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DUVAS;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.IRepository;

namespace API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : ODataController
    {
        private readonly IBuildingRepository _buildingRepository;

        public BuildingsController(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        // GET: odata/Buildings
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildings(string searchTerm = null)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                return Ok(await _buildingRepository.GetBuildingsAsync());
            }

            var buildings = await _buildingRepository.SearchBuildingsAsync(searchTerm);
            return Ok(buildings);
        }

        // GET: odata/Buildings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetBuilding(int id)
        {
            var building = await _buildingRepository.GetBuildingByIdAsync(id);
            if (building == null)
            {
                return BadRequest();
            }

            return Ok(building);
        }

        // POST: odata/Buildings
        [HttpPost]
        public async Task<ActionResult<Building>> PostBuilding([FromBody] Building building)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Lưu sách vào cơ sở dữ liệu
                await _buildingRepository.SaveBuildingAsync(building);

                return CreatedAtAction(nameof(GetBuilding), new { id = building.BuildingId }, building);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error in PostBuilding: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PUT: odata/Buildings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(int id, [FromBody] Building building)
        {
            if (id != building.BuildingId)
            {
                return BadRequest();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _buildingRepository.UpdateBuildingAsync(building);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BuildingExists(id))
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

        // DELETE: odata/Buildings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            var building = await _buildingRepository.GetBuildingByIdAsync(id);
            if (building == null)
            {
                return BadRequest();
            }

            await _buildingRepository.DeleteBuildingAsync(building);
            return NoContent();
        }
        private async Task<bool> BuildingExists(int id)
        {
            var Building = await _buildingRepository.GetBuildingByIdAsync(id);
            return Building != null;
        }
    }
}
