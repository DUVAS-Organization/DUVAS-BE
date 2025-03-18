using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors; // Thêm để dùng CORS

namespace DUVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowLocalhost3000")] // Bật CORS với policy
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> SearchRooms(
            [FromQuery] int? categoryRoomId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] double? minArea = null,
            [FromQuery] double? maxArea = null,
            [FromQuery] string location = "")
        {
            try
            {
                var query = _context.Rooms
                    .Include(r => r.User)
                    .Include(r => r.CategoryRoom)
                    .AsQueryable();

                if (categoryRoomId.HasValue)
                    query = query.Where(r => r.CategoryRoomId == categoryRoomId.Value);
                if (minPrice.HasValue)
                    query = query.Where(r => r.Price >= minPrice.Value);
                if (maxPrice.HasValue)
                    query = query.Where(r => r.Price <= maxPrice.Value);
                if (minArea.HasValue)
                    query = query.Where(r => r.Acreage >= minArea.Value);
                if (maxArea.HasValue)
                    query = query.Where(r => r.Acreage <= maxArea.Value);
                if (!string.IsNullOrEmpty(location))
                    query = query.Where(r => r.LocationDetail.Contains(location));

                var rooms = await query.ToListAsync();
                return Ok(rooms.Any() ? rooms : new List<Room>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tìm kiếm phòng: {ex.Message}");
            }
        }

        [HttpGet("serviceposts")]
        public async Task<ActionResult<IEnumerable<ServicePost>>> SearchServicePosts(
            [FromQuery] int? categoryServiceId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string location = "")
        {
            try
            {
                var query = _context.ServicePosts
                    .Include(sp => sp.User)
                    .Include(sp => sp.CategoryService)
                    .AsQueryable();

                if (categoryServiceId.HasValue)
                    query = query.Where(sp => sp.CategoryServiceId == categoryServiceId.Value);
                if (minPrice.HasValue)
                    query = query.Where(sp => sp.Price >= minPrice.Value);
                if (maxPrice.HasValue)
                    query = query.Where(sp => sp.Price <= maxPrice.Value);
                if (!string.IsNullOrEmpty(location))
                    query = query.Where(sp => sp.Location.Contains(location));

                var servicePosts = await query.ToListAsync();
                return Ok(servicePosts.Any() ? servicePosts : new List<ServicePost>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tìm kiếm bài đăng dịch vụ: {ex.Message}");
            }
        }
    }
}