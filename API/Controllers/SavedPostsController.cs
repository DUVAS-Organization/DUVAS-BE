using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DUVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SavedPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Lấy danh sách bài đã lưu của một user
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSavedPosts(int userId)
        {
            try
            {
                var savedPosts = await _context.SavedPosts
                    .Where(sp => sp.UserId == userId) // Chỉ lấy bài đăng của user
                    .Select(sp => new
                    {
                        sp.RoomId,
                        sp.UserId,
                        sp.SavedAt,
                        Room = new
                        {
                            sp.Room.Title,
                            sp.Room.Price,
                            sp.Room.Acreage,
                            sp.Room.LocationDetail,
                            sp.Room.Image
                        }
                    })
                    .ToListAsync();

                return Ok(savedPosts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách bài đăng!", error = ex.Message });
            }
        }

        // 🔹 Thêm hoặc xóa bài đã lưu
        [HttpPost]
        public async Task<IActionResult> ToggleSavePost([FromBody] SavePostRequest request)
        {
            if (request.RoomId == 0 || request.UserId == 0)
            {
                return BadRequest(new { message = "RoomId hoặc UserId không hợp lệ!" });
            }

            var existingPost = await _context.SavedPosts
                .FirstOrDefaultAsync(sp => sp.RoomId == request.RoomId && sp.UserId == request.UserId);

            if (existingPost != null)
            {
                _context.SavedPosts.Remove(existingPost);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Đã bỏ lưu bài đăng.", status = "removed" });
            }
            else
            {
                var newSavedPost = new SavedPost
                {
                    RoomId = request.RoomId,
                    UserId = request.UserId, // Quan trọng!
                    SavedAt = DateTime.Now
                };

                _context.SavedPosts.Add(newSavedPost);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Đã lưu bài đăng thành công.", status = "saved" });
            }
        }
    }

    public class SavePostRequest
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
    }
}
