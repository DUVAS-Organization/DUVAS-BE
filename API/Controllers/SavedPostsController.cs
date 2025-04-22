using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using DUVAS;
using API.Controllers;
using API.Hubs;

namespace DUVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<SavedPostHub> _hubContext; // Thêm IHubContext để gửi thông báo

        public SavedPostsController(ApplicationDbContext context, IHubContext<SavedPostHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // 🔹 Lấy danh sách bài đã lưu của một user (bao gồm cả Room và ServicePost)
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSavedPosts(int userId)
        {
            try
            {
                var savedPosts = await _context.SavedPosts
                    .Where(sp => sp.UserId == userId)
                    .Select(sp => new
                    {
                        sp.RoomId,
                        sp.ServicePostId,
                        sp.UserId,
                        sp.SavedAt,
                        Room = sp.Room != null ? new
                        {
                            sp.Room.Title,
                            sp.Room.Price,
                            sp.Room.Acreage,
                            sp.Room.LocationDetail,
                            sp.Room.Image
                        } : null,
                        ServicePost = sp.ServicePost != null ? new
                        {
                            sp.ServicePost.Title,
                            sp.ServicePost.Price,
                            sp.ServicePost.PhoneNumber,
                            sp.ServicePost.Location,
                            sp.ServicePost.Description,
                            sp.ServicePost.Image
                        } : null
                    })
                    .ToListAsync();

                return Ok(savedPosts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách bài đăng!", error = ex.Message });
            }
        }

        // 🔹 Thêm hoặc xóa bài đã lưu cho Room hoặc ServicePost
        [HttpPost]
        public async Task<IActionResult> ToggleSavePost([FromBody] SavePostRequest request)
        {
            // Kiểm tra UserId
            if (request.UserId == 0)
            {
                return BadRequest(new { message = "UserId không hợp lệ!" });
            }
            // Cần có ít nhất một trong 2: RoomId hoặc ServicePostId
            if (request.RoomId == 0 && request.ServicePostId == 0)
            {
                return BadRequest(new { message = "Cần cung cấp RoomId hoặc ServicePostId!" });
            }

            // Nếu có RoomId (ưu tiên xử lý Room nếu có)
            if (request.RoomId > 0)
            {
                var existingPost = await _context.SavedPosts
                    .FirstOrDefaultAsync(sp => sp.RoomId == request.RoomId && sp.UserId == request.UserId);

                if (existingPost != null)
                {
                    _context.SavedPosts.Remove(existingPost);
                    await _context.SaveChangesAsync();

                    // Gửi thông báo qua SignalR
                    await _hubContext.Clients.Group(request.UserId.ToString())
                        .SendAsync("savedPostRemoved", new
                        {
                            data = new
                            {
                                userId = request.UserId,
                                roomId = request.RoomId
                            }
                        });

                    return Ok(new { message = "Đã bỏ lưu bài đăng.", status = "removed" });
                }
                else
                {
                    var newSavedPost = new SavedPost
                    {
                        RoomId = request.RoomId,
                        UserId = request.UserId,
                        SavedAt = DateTime.UtcNow
                    };

                    _context.SavedPosts.Add(newSavedPost);
                    await _context.SaveChangesAsync();

                    // Lấy thông tin Room để gửi kèm
                    var room = await _context.Rooms
                        .Where(r => r.UserId == request.RoomId)
                        .Select(r => new
                        {
                            r.Title,
                            r.Price,
                            r.Acreage,
                            r.LocationDetail,
                            r.Image
                        })
                        .FirstOrDefaultAsync();

                    // Gửi thông báo qua SignalR
                    await _hubContext.Clients.Group(request.UserId.ToString())
                        .SendAsync("savedPostAdded", new
                        {
                            data = new
                            {
                                userId = request.UserId,
                                roomId = request.RoomId,
                                savedAt = newSavedPost.SavedAt,
                                room
                            }
                        });

                    return Ok(new { message = "Đã lưu bài đăng thành công.", status = "saved" });
                }
            }
            // Nếu không có RoomId mà có ServicePostId
            else if (request.ServicePostId > 0)
            {
                var existingPost = await _context.SavedPosts
                    .FirstOrDefaultAsync(sp => sp.ServicePostId == request.ServicePostId && sp.UserId == request.UserId);

                if (existingPost != null)
                {
                    _context.SavedPosts.Remove(existingPost);
                    await _context.SaveChangesAsync();

                    // Gửi thông báo qua SignalR
                    await _hubContext.Clients.Group(request.UserId.ToString())
                        .SendAsync("savedPostRemoved", new
                        {
                            data = new
                            {
                                userId = request.UserId,
                                servicePostId = request.ServicePostId
                            }
                        });

                    return Ok(new { message = "Đã bỏ lưu bài đăng.", status = "removed" });
                }
                else
                {
                    var newSavedPost = new SavedPost
                    {
                        ServicePostId = request.ServicePostId,
                        UserId = request.UserId,
                        SavedAt = DateTime.UtcNow
                    };

                    _context.SavedPosts.Add(newSavedPost);
                    await _context.SaveChangesAsync();

                    // Lấy thông tin ServicePost để gửi kèm
                    var servicePost = await _context.ServicePosts
                        .Where(sp => sp.UserId == request.ServicePostId)
                        .Select(sp => new
                        {
                            sp.Title,
                            sp.Price,
                            sp.PhoneNumber,
                            sp.Location,
                            sp.Description,
                            sp.Image
                        })
                        .FirstOrDefaultAsync();

                    // Gửi thông báo qua SignalR
                    await _hubContext.Clients.Group(request.UserId.ToString())
                        .SendAsync("savedPostAdded", new
                        {
                            data = new
                            {
                                userId = request.UserId,
                                servicePostId = request.ServicePostId,
                                savedAt = newSavedPost.SavedAt,
                                servicePost
                            }
                        });

                    return Ok(new { message = "Đã lưu bài đăng thành công.", status = "saved" });
                }
            }

            return BadRequest(new { message = "Yêu cầu không hợp lệ." });
        }
    }

    public class SavePostRequest
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int ServicePostId { get; set; }
    }
}