using DUVAS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.IRepository;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ODataController
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IServicePostRepository _servicePostRepository;

        public HomePageController(IRoomRepository roomRepository, IServicePostRepository servicePostRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _servicePostRepository = servicePostRepository ?? throw new ArgumentNullException(nameof(servicePostRepository));
        }

        // GET: api/HomePage/rooms
        [EnableQuery]
        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetUserRooms(string searchTerm = null)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Ok(await _roomRepository.GetRoomsAsync());
            }

            var rooms = await _roomRepository.SearchRoomsAsync(searchTerm);
            return Ok(rooms);
        }

        // GET: api/HomePage/rooms/{id}
        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<Room>> GetUserRoom(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound(); // Sử dụng NotFound thay vì BadRequest
            }

            return Ok(room);
        }

        // GET: api/HomePage/service-posts
        [EnableQuery]
        [HttpGet("service-posts")]
        public async Task<ActionResult<IEnumerable<ServicePost>>> GetUserServicePosts(string searchTerm = null)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Ok(await _servicePostRepository.GetServicePostsAsync());
            }

            var servicePosts = await _servicePostRepository.SearchServicePostsAsync(searchTerm);
            return Ok(servicePosts);
        }

        // GET: api/HomePage/service-posts/{id}
        [HttpGet("service-posts/{id}")]
        public async Task<ActionResult<ServicePost>> GetUserServicePost(int id)
        {
            var servicePost = await _servicePostRepository.GetServicePostByIdAsync(id);
            if (servicePost == null)
            {
                return NotFound(); // Sử dụng NotFound thay vì BadRequest
            }

            return Ok(servicePost);
        }
    }
}
