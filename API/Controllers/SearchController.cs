using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DUVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/search/rooms?categoryRoomId=1&minPrice=1000000&maxPrice=5000000&minArea=20&maxArea=50&location=Quận 7&status=1&isPermission=1
        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> SearchRooms(
            [FromQuery] int? categoryRoomId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] double? minArea = null,
            [FromQuery] double? maxArea = null,
            [FromQuery] string location = "",
            [FromQuery] int? status = null,
            [FromQuery] int? isPermission = null)
        {
            try
            {
                var query = _context.Rooms
                    .Include(r => r.User)           // Bao gồm thông tin người đăng
                    .Include(r => r.CategoryRoom)   // Bao gồm thông tin danh mục
                    .AsQueryable();

                // Lọc theo CategoryRoomId
                if (categoryRoomId.HasValue)
                {
                    query = query.Where(r => r.CategoryRoomId == categoryRoomId.Value);
                }

                // Lọc theo mức giá
                if (minPrice.HasValue)
                {
                    query = query.Where(r => r.Price >= minPrice.Value);
                }
                if (maxPrice.HasValue)
                {
                    query = query.Where(r => r.Price <= maxPrice.Value);
                }

                // Lọc theo diện tích
                if (minArea.HasValue)
                {
                    query = query.Where(r => r.Acreage >= minArea.Value);
                }
                if (maxArea.HasValue)
                {
                    query = query.Where(r => r.Acreage <= maxArea.Value);
                }

                // Lọc theo vị trí (tìm kiếm gần đúng)
                if (!string.IsNullOrEmpty(location))
                {
                    query = query.Where(r => r.LocationDetail.Contains(location));
                }


                // Lấy danh sách phòng đã lọc
                var rooms = await query.ToListAsync();

                if (!rooms.Any())
                {
                    return Ok(new List<Room>()); // Trả về mảng rỗng nếu không tìm thấy
                }

                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tìm kiếm phòng: {ex.Message}");
            }
        }

        // GET: api/search/serviceposts?categoryServiceId=1&minPrice=50000&maxPrice=200000&location=Quận 1&phoneNumber=0909
        [HttpGet("serviceposts")]
        public async Task<ActionResult<IEnumerable<ServicePost>>> SearchServicePosts(
            [FromQuery] int? categoryServiceId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string location = "",
            [FromQuery] string phoneNumber = "")
        {
            try
            {
                var query = _context.ServicePosts
                    .Include(sp => sp.User)           // Bao gồm thông tin người đăng
                    .Include(sp => sp.CategoryService) // Bao gồm thông tin danh mục dịch vụ
                    .AsQueryable();

                // Lọc theo CategoryServiceId
                if (categoryServiceId.HasValue)
                {
                    query = query.Where(sp => sp.CategoryServiceId == categoryServiceId.Value);
                }

                // Lọc theo mức giá
                if (minPrice.HasValue)
                {
                    query = query.Where(sp => sp.Price >= minPrice.Value);
                }
                if (maxPrice.HasValue)
                {
                    query = query.Where(sp => sp.Price <= maxPrice.Value);
                }

                // Lọc theo vị trí (tìm kiếm gần đúng)
                if (!string.IsNullOrEmpty(location))
                {
                    query = query.Where(sp => sp.Location.Contains(location));
                }

                // Lọc theo số điện thoại (tìm kiếm gần đúng)
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    query = query.Where(sp => sp.PhoneNumber.Contains(phoneNumber));
                }

                // Lấy danh sách bài đăng dịch vụ đã lọc
                var servicePosts = await query.ToListAsync();

                if (!servicePosts.Any())
                {
                    return Ok(new List<ServicePost>()); // Trả về mảng rỗng nếu không tìm thấy
                }

                return Ok(servicePosts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tìm kiếm bài đăng dịch vụ: {ex.Message}");
            }
        }
    }
}