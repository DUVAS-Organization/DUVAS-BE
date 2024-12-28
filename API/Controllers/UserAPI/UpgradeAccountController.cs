using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Threading.Tasks;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpgradeAccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UpgradeAccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // API: Nâng cấp RoleLandlord
        [HttpPut("{id}/Landlord")]
        public async Task<IActionResult> UpgradeToLandlord(int id)
        {
            try
            {
                // Tìm user trong database
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User không tồn tại.");
                }

                // Cập nhật RoleLandlord
                user.RoleLandlord = 1; // 1 là giá trị định nghĩa rằng user là landlord
                await _userRepository.UpdateUserAsync(user);

                return NoContent(); // Thành công, không cần trả dữ liệu
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi server khi xử lý yêu cầu.");
            }
        }

        // API: Nâng cấp RoleService
        [HttpPut("{id}/Service")]
        public async Task<IActionResult> UpgradeToService(int id)
        {
            try
            {
                // Tìm user trong database
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User không tồn tại.");
                }

                // Cập nhật RoleService
                user.RoleService = 1; // 1 là giá trị định nghĩa rằng user là service provider
                await _userRepository.UpdateUserAsync(user);

                return NoContent(); // Thành công, không cần trả dữ liệu
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi server khi xử lý yêu cầu.");
            }
        }
    }
}
