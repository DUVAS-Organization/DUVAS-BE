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
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DTO;

namespace API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ODataController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: odata/Users
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string searchTerm = null)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                return Ok(await _userRepository.GetUsersAsync());
            }

            var users = await _userRepository.SearchUsersAsync(searchTerm);
            return Ok(users);
        }

        [HttpGet("locked-users")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetLockedUsers()
        {
            try
            {
                var lockedUsers = await _userRepository.GetListUserLockAsync();
                return Ok(lockedUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách người dùng bị khóa: {ex.Message}");
            }
        }
        [HttpGet("active-users")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetListUserActiveAsync()
        {
            try
            {
                var activeUsers = await _userRepository.GetListUserActiveAsync();
                return Ok(activeUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách người dùng active: {ex.Message}");
            }
        }

        // GET: odata/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        // POST: odata/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Lưu sách vào cơ sở dữ liệu
                await _userRepository.SaveUserAsync(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error in PostRoom: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PUT: odata/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _userRepository.UpdateUserAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
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

        // DELETE: odata/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }

            await _userRepository.DeleteUserAsync(user);
            return NoContent();
        }
        private async Task<bool> UserExists(int id)
        {
            var User = await _userRepository.GetUserByIdAsync(id);
            return User != null;
        }
        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            try
            {
                await _userRepository.LockUserAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa user: {ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnLockUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            try
            {
                await _userRepository.UnLockUserAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi khóa user: {ex.Message}");
            }

            return NoContent();
        }
        [HttpPut("acceptUpRoleLandLord/{id}")]
        public async Task<IActionResult> AcceptUpRoleLandLordAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            try
            {
                await _userRepository.AcceptUpRoleLandLordAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xác nhận UpRole LandLord: {ex.Message}");
            }

            return NoContent();
        }
        [HttpGet("upRole-Service")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetListUpRoleService()
        {
            try
            {
                var uproleService = await _userRepository.GetListUpRoleService();
                return Ok(uproleService);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách người dùng uprole-Service: {ex.Message}");
            }
        }
        [HttpPut("acceptUpRoleService/{id}")]
        public async Task<IActionResult> AcceptUpRoleServiceAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            try
            {
                await _userRepository.AcceptUpRoleServiceAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xác nhận UpRole LandLord: {ex.Message}");
            }

            return NoContent();
        }

       
        [HttpGet("upRole-LandLord")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetListUpRoleLandLord()
        {
            try
            {
                var uproleLandLord = await _userRepository.GetListUpRoleLandLord();
                return Ok(uproleLandLord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách người dùng uprole-LandLord: {ex.Message}");
            }
        }

    }
}
