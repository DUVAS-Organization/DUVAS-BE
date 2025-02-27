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
    public class CategoryPriorityPackageServicePostsController : ODataController
    {
        private readonly ICategoryPriorityPackageServicePostRepository _categoryPriorityPackageServicePostRepository;

        public CategoryPriorityPackageServicePostsController(ICategoryPriorityPackageServicePostRepository categoryPriorityPackageServicePostRepository)
        {
            _categoryPriorityPackageServicePostRepository = categoryPriorityPackageServicePostRepository;
        }

        // GET: odata/CategoryPriorityPackageServicePosts
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryPriorityPackageServicePostDTO>>> GetCategoryPriorityPackageServicePosts()
        {
            var categoryPriorityPackageServicePosts = await _categoryPriorityPackageServicePostRepository.GetCategoryPriorityPackageServicePostsAsync();
            return Ok(categoryPriorityPackageServicePosts);
        }

        // GET: odata/CategoryPriorityPackageServicePosts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryPriorityPackageServicePost>> GetCategoryPriorityPackageServicePost(int id)
        {
            var categoryPriorityPackageServicePost = await _categoryPriorityPackageServicePostRepository.GetCategoryPriorityPackageServicePostByIdAsync(id);
            if (categoryPriorityPackageServicePost == null)
            {
                return NotFound();
            }

            return Ok(categoryPriorityPackageServicePost);
        }

        // POST: odata/CategoryPriorityPackageServicePosts
        [HttpPost]
        public async Task<ActionResult<CategoryPriorityPackageServicePost>> PostCategoryPriorityPackageServicePost([FromBody] CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
        {
            await _categoryPriorityPackageServicePostRepository.SaveCategoryPriorityPackageServicePostAsync(categoryPriorityPackageServicePost);
            return CreatedAtAction(nameof(GetCategoryPriorityPackageServicePost), new { id = categoryPriorityPackageServicePost.CategoryPriorityPackageServicePostId }, categoryPriorityPackageServicePost);
        }

        // PUT: odata/CategoryPriorityPackageServicePosts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryPriorityPackageServicePost(int id, [FromBody] CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
        {
            if (id != categoryPriorityPackageServicePost.CategoryPriorityPackageServicePostId)
            {
                return BadRequest();
            }

            try
            {
                await _categoryPriorityPackageServicePostRepository.UpdateCategoryPriorityPackageServicePostAsync(categoryPriorityPackageServicePost);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryPriorityPackageServicePostExists(id))
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

        // DELETE: odata/CategoryPriorityPackageServicePosts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryPriorityPackageServicePost(int id)
        {
            var categoryPriorityPackageServicePost = await _categoryPriorityPackageServicePostRepository.GetCategoryPriorityPackageServicePostByIdAsync(id);
            if (categoryPriorityPackageServicePost == null)
            {
                return NotFound();
            }

            await _categoryPriorityPackageServicePostRepository.DeleteCategoryPriorityPackageServicePostAsync(categoryPriorityPackageServicePost);
            return NoContent();
        }

        private async Task<bool> CategoryPriorityPackageServicePostExists(int id)
        {
            var categoryPriorityPackageServicePost = await _categoryPriorityPackageServicePostRepository.GetCategoryPriorityPackageServicePostByIdAsync(id);
            return categoryPriorityPackageServicePost != null;
        }
    }
}
