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
    public class PriorityPackageServicePostController : ControllerBase
    {
        private readonly IPriorityPackageServicePostRepository _repository;

        public PriorityPackageServicePostController(IPriorityPackageServicePostRepository repository)
        {
            _repository = repository;
        }

        // Lấy danh sách tất cả PriorityPackageServicePost
        [HttpGet]
        public async Task<ActionResult<List<PriorityPackageServicePostDTO>>> GetAll()
        {
            try
            {
                var ServicePosts = await _repository.GetPriorityPackageServicePostsAsync();
                return Ok(ServicePosts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Lấy PriorityPackageServicePost theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PriorityPackageServicePostDTO>> GetById(int id)
        {
            try
            {
                var ServicePost = await _repository.FindPriorityPackageServicePostByIdAsync(id);
                return Ok(ServicePost);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Thêm mới PriorityPackageServicePost
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PriorityPackageServicePost package)
        {
            try
            {
                await _repository.SavePriorityPackageServicePostAsync(package);
                return CreatedAtAction(nameof(GetById), new { id = package.PriorityPackageServicePostId }, package);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Cập nhật PriorityPackageServicePost
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PriorityPackageServicePost package)
        {
            if (id != package.PriorityPackageServicePostId)
            {
                return BadRequest(new { message = "ID không khớp." });
            }

            try
            {
                await _repository.UpdatePriorityPackageServicePostAsync(package);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Xóa PriorityPackageServicePost
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeletePriorityPackageServicePostAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
