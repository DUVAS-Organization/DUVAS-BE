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
    public class ServicePostsController : ODataController
    {
        private readonly IServicePostRepository _servicePostRepository;

        public ServicePostsController(IServicePostRepository ServicePostRepository)
        {
            _servicePostRepository = ServicePostRepository;
        }

        // GET: odata/ServicePosts
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicePost>>> GetServicePosts(string searchTerm = null)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                return Ok(await _servicePostRepository.GetServicePostsAsync());
            }

            var ServicePosts = await _servicePostRepository.SearchServicePostsAsync(searchTerm);
            return Ok(ServicePosts);
        }

        // GET: odata/ServicePosts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicePost>> GetServicePost(int id)
        {
            var ServicePost = await _servicePostRepository.GetServicePostByIdAsync(id);
            if (ServicePost == null)
            {
                return BadRequest();
            }

            return Ok(ServicePost);
        }

        // POST: odata/ServicePosts
        [HttpPost]
        public async Task<ActionResult<ServicePost>> PostServicePost([FromBody] ServicePost ServicePost)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Lưu sách vào cơ sở dữ liệu
                await _servicePostRepository.SaveServicePostAsync(ServicePost);

                return CreatedAtAction(nameof(GetServicePost), new { id = ServicePost.ServicePostId }, ServicePost);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error in PostServicePost: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PUT: odata/ServicePosts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicePost(int id, [FromBody] ServicePost ServicePost)
        {
            if (id != ServicePost.ServicePostId)
            {
                return BadRequest();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _servicePostRepository.UpdateServicePostAsync(ServicePost);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ServicePostExists(id))
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

        // DELETE: odata/ServicePosts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicePost(int id)
        {
            var ServicePost = await _servicePostRepository.GetServicePostByIdAsync(id);
            if (ServicePost == null)
            {
                return BadRequest();
            }

            await _servicePostRepository.DeleteServicePostAsync(ServicePost);
            return NoContent();
        }
        private async Task<bool> ServicePostExists(int id)
        {
            var ServicePost = await _servicePostRepository.GetServicePostByIdAsync(id);
            return ServicePost != null;
        }

    }
}
