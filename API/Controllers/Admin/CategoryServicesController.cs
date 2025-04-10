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
    public class CategoryServicesController : ODataController
    {
        private readonly ICategoryServiceRepository _categoryServiceRepository;

        public CategoryServicesController(ICategoryServiceRepository categoryServiceRepository)
        {
            _categoryServiceRepository = categoryServiceRepository;
        }

        // GET: odata/Category
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryServiceDTO>>> GetCategoryServices()
        {
            var categoryServices = await _categoryServiceRepository.GetCategoryServicesAsync();
            return Ok(categoryServices);
        }

        // GET: odata/Category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryService>> GetCategoryService(int id)
        {
            var categoryServices = await _categoryServiceRepository.GetCategoryServiceByIdAsync(id);
            if (categoryServices == null)
            {
                return BadRequest();
            }

            return Ok(categoryServices);
        }

        // POST: odata/Category
        [HttpPost]
        public async Task<ActionResult<CategoryService>> PostCategoryService([FromBody] CategoryService categoryServices)
        {
            await _categoryServiceRepository.SaveCategoryServiceAsync(categoryServices);
            return CreatedAtAction(nameof(GetCategoryService), new { id = categoryServices.CategoryServiceId }, categoryServices);
        }

        // PUT: odata/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryService(int id, [FromBody] CategoryService categoryServices)
        {
            if (id != categoryServices.CategoryServiceId)
            {
                return BadRequest();
            }

            try
            {
                await _categoryServiceRepository.UpdateCategoryServiceAsync(categoryServices);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryServiceExists(id))
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
        public async Task<IActionResult> DeleteCategoryService(int id)
        {
            var categoryServices = await _categoryServiceRepository.GetCategoryServiceByIdAsync(id);
            if (categoryServices == null)
            {
                return BadRequest();
            }

            await _categoryServiceRepository.DeleteCategoryServiceAsync(categoryServices);
            return NoContent();
        }
        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockCategoryService(int id)
        {
            var categoryServices = await _categoryServiceRepository.GetCategoryServiceByIdAsync(id);
            if (categoryServices == null)
            {
                return NotFound("Service không tồn tại.");
            }

            try
            {
                await _categoryServiceRepository.LockCategoryService(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa Service: {ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnLockCategoryService(int id)
        {
            var categoryServices = await _categoryServiceRepository.GetCategoryServiceByIdAsync(id);
            if (categoryServices == null)
            {
                return NotFound("Service không tồn tại.");
            }

            try
            {
                await _categoryServiceRepository.UnLockCategoryService(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa Service: {ex.Message}");
            }

            return NoContent();
        }
        private async Task<bool> CategoryServiceExists(int id)
        {
            var categoryServices = await _categoryServiceRepository.GetCategoryServiceByIdAsync(id);
            return categoryServices != null;
        }

    }
}
