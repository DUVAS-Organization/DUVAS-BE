using DataAccess;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.ServiceOwner
{
    [Route("api/landlord/[controller]")]
    [ApiController]
    public class ServiceManagementController : ControllerBase
    {
        private readonly IServicePostRepository _servicePostRepository;
        private readonly IRentalServiceListRepository _rentalServiceListRepository;
        private readonly IPriorityPackageServicePostRepository _priorityPackageServicePostRepository;

        public ServiceManagementController(
            IServicePostRepository servicePostRepository,
            IRentalServiceListRepository rentalServiceListRepository,
            IPriorityPackageServicePostRepository priorityPackageServicePostRepository)
        {
            _servicePostRepository = servicePostRepository;
            _rentalServiceListRepository = rentalServiceListRepository;
            _priorityPackageServicePostRepository = priorityPackageServicePostRepository;
        }

        private int GetServiceId()
        {
            var userIdClaim = User.FindFirst("UserId");
            var serviceId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
            Console.WriteLine($"ServiceId: {serviceId}");
            return serviceId;
        }

        private async Task<bool> IsService(int userId)
        {
            var user = await UserDAO.FindUserByIdAsync(userId);
            Console.WriteLine($"UserId: {userId}, RoleService: {user?.RoleService}");
            return user?.RoleService == 1;
        }
        [HttpGet("my-services")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> GetMyServices()
        {
            int userId = GetServiceId();
            if (!await IsService(userId))
            {
                return Unauthorized("Bạn không có quyền sử dụng chức năng này.");
            }

            var services = await _servicePostRepository.GetServicePostsByUserIdAsync(userId);

            if (services == null || !services.Any())
            {
                return NotFound("Bạn chưa đăng dịch vụ nào.");
            }

            return Ok(new { message = "Danh sách dịch vụ của bạn", services });
        }


        #region Add New Service
        [HttpPost("add-service")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> AddNewService([FromBody] ServicePostDTO servicePostDto)
        {
            int userId = GetServiceId();
            if (!await IsService(userId))
            {
                return Unauthorized("Bạn không có quyền sử dụng chức năng này.");
            }

            try
            {
                var servicePost = new ServicePost
                {
                    UserId = userId,
                    Title = servicePostDto.Title,
                    PhoneNumber = servicePostDto.PhoneNumber,
                    Price = servicePostDto.Price,
                    Location = servicePostDto.Location,
                    Description = servicePostDto.Description,
                    Image = servicePostDto.Image,
                    CategoryServiceId = servicePostDto.CategoryServiceId,
                    IsPermission = servicePostDto.IsPermission ?? 1, // hoặc giá trị mặc định mà bạn muốn

                };

                await _servicePostRepository.SaveServicePostAsync(servicePost);

                return Ok(new { message = "Service added successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while adding the service.", error = ex.Message });
            }
        }
        #endregion

        #region Edit Service
        [HttpPut("edit-service/{servicePostId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> EditService(int servicePostId, [FromBody] ServicePostDTO servicePostDto)
        {
            int userId = GetServiceId();
            if (!await IsService(userId))
            {
                return Unauthorized("Bạn không có quyền sử dụng chức năng này.");
            }

            try
            {
                var servicePost = await _servicePostRepository.GetServicePostByIdAsync(servicePostId);

                if (servicePost == null)
                {
                    return NotFound(new { message = "Service not found." });
                }

                servicePost.Title = servicePostDto.Title;
                servicePost.PhoneNumber = servicePostDto.PhoneNumber;
                servicePost.Price = servicePostDto.Price;
                servicePost.Location = servicePostDto.Location;
                servicePost.Description = servicePostDto.Description;
                servicePost.Image = servicePostDto.Image;
                servicePost.CategoryServiceId = servicePostDto.CategoryServiceId;
                servicePost.IsPermission = servicePostDto.IsPermission ?? servicePost.IsPermission; // ✅ thêm dòng này


                await _servicePostRepository.UpdateServicePostAsync(servicePost);

                return Ok(new { message = "Service updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while editing the service.", error = ex.Message });
            }
        }
        #endregion

        #region Delete Service
        [HttpDelete("delete-service/{servicePostId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> DeleteService(int servicePostId)
        {
            int userId = GetServiceId();
            if (!await IsService(userId))
            {
                return Unauthorized("Bạn không có quyền sử dụng chức năng này.");
            }

            try
            {
                var servicePost = await _servicePostRepository.GetServicePostByIdAsync(servicePostId);

                if (servicePost == null)
                {
                    return NotFound(new { message = "Service not found." });
                }

                await _servicePostRepository.DeleteServicePostAsync(servicePost);

                return Ok(new { message = "Service deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while deleting the service.", error = ex.Message });
            }
        }
        #endregion

        #region View Service List
        [HttpGet("view-service-list")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> ViewServiceList()
        {
            int userId = GetServiceId();
            if (!await IsService(userId))
            {
                return Unauthorized("Bạn không có quyền sử dụng chức năng này.");
            }

            try
            {
                var services = await _servicePostRepository.GetServicePostsAsync();

                if (services == null || !services.Any())
                {
                    return NotFound(new { message = "No services found." });
                }

                return Ok(new { message = "List of services", services });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching the service list.", error = ex.Message });
            }
        }
        #endregion

        #region Track Service Status
        [HttpGet("track-service-status/{servicePostId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> TrackServiceStatus(int servicePostId)
        {
            int userId = GetServiceId();
            if (!await IsService(userId))
            {
                return Unauthorized("Bạn không có quyền sử dụng chức năng này.");
            }

            try
            {
                var service = await _servicePostRepository.GetServicePostByIdAsync(servicePostId);

                if (service == null)
                {
                    return NotFound(new { message = "Service not found." });
                }

                var rentalService = await _rentalServiceListRepository.GetRentalServiceListsAsync();
                var priorityPackageServicePost = await _priorityPackageServicePostRepository.GetPriorityPackageServicePostsAsync();

                return Ok(new
                {
                    message = "Service status fetched successfully!",
                    service,
                    rentalService,
                    priorityPackageServicePost
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching the service status.", error = ex.Message });
            }
        }
        #endregion
    }
}
