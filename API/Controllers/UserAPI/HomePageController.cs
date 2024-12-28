using DUVAS;
using Microsoft.AspNetCore.Http;
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
            _roomRepository = roomRepository;
            _servicePostRepository = servicePostRepository;
        }

        // GET: odata/Rooms
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(string searchTerm = null)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                return Ok(await _roomRepository.GetRoomsAsync());
            }

            var rooms = await _roomRepository.SearchRoomsAsync(searchTerm);
            return Ok(rooms);
        }

        // GET: odata/Rooms/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return BadRequest();
            }

            return Ok(room);
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

            var servicePosts = await _servicePostRepository.SearchServicePostsAsync(searchTerm);
            return Ok(servicePosts);
        }

        // GET: odata/ServicePosts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicePost>> GetServicePost(int id)
        {
            var servicePost = await _servicePostRepository.GetServicePostByIdAsync(id);
            if (servicePost == null)
            {
                return BadRequest();
            }

            return Ok(servicePost);
        }


    }
}
