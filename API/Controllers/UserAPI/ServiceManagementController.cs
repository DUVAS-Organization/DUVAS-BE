using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DUVAS;
using Repositories.IRepository;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceManagementController : ControllerBase
    {
        private readonly IRentalServiceListRepository _rentalServiceListRepository;
        private readonly IServiceFeedbackRepository _serviceFeedbackRepository;

        public ServiceManagementController(IRentalServiceListRepository rentalServiceListRepository, IServiceFeedbackRepository serviceFeedbackRepository)
        {
            _rentalServiceListRepository = rentalServiceListRepository;
            _serviceFeedbackRepository = serviceFeedbackRepository;
        }

        /// <summary>
        /// Rent a service
        /// </summary>
        [HttpPost("rent-service")]
        public async Task<IActionResult> RentService([FromBody] RentalServiceList rentalRequest)
        {
            if (rentalRequest == null || rentalRequest.ServicePostID <= 0 || rentalRequest.RenterID <= 0)
            {
                return BadRequest("Invalid rental request.");
            }

            rentalRequest.RentalServiceStatus = 0; // New rental request
            rentalRequest.CreationDateTime = DateTime.UtcNow;
            await _rentalServiceListRepository.SaveRentalServiceListAsync(rentalRequest);
            return Ok("Service rental request created successfully.");
        }

        /// <summary>
        /// View service reviews
        /// </summary>
        [HttpGet("service-reviews/{servicePostId}")]
        public async Task<IActionResult> ViewServiceReviews(int servicePostId)
        {
            // Lấy danh sách feedback liên quan đến ServicePostId
            var feedbacks = await _serviceFeedbackRepository.GetServiceFeedbacksAsync();
            var serviceFeedbacks = feedbacks.Where(f => f.ServicePostId == servicePostId).ToList();

            if (!serviceFeedbacks.Any())
            {
                return NotFound("No reviews found for the specified service.");
            }

            return Ok(serviceFeedbacks.Select(f => new
            {
                f.ServiceFeedbackId,
                f.ServicePostId,
                f.Comment,
                f.Star,
                f.Image
            }));
        }

        /// <summary>
        /// View service rental status
        /// </summary>
        [HttpGet("rental-status/{servicePostId}")]
        public async Task<IActionResult> ViewServiceRentalStatus(int servicePostId)
        {
            var rentals = await _rentalServiceListRepository.GetRentalServiceListsAsync();
            var serviceRentals = rentals.FindAll(r => r.ServicePostId == servicePostId);

            if (!serviceRentals.Any())
            {
                return NotFound("No rentals found for the specified service.");
            }

            return Ok(serviceRentals);
        }

        /// <summary>
        /// Cancel a service rental
        /// </summary>
        [HttpPut("cancel-service/{rentalServiceId}")]
        public async Task<IActionResult> CancelService(int rentalServiceId)
        {
            try
            {
                // Tìm RentalService theo ID
                var rentalService = await _rentalServiceListRepository.GetRentalServiceListByIdAsync(rentalServiceId);

                if (rentalService == null)
                {
                    return NotFound("Service rental not found.");
                }

                // Đổi trạng thái thành "Đã hủy"
                rentalService.RentalServiceStatus = -1; // -1: Cancelled
                await _rentalServiceListRepository.UpdateRentalServiceListAsync(rentalService);

                return Ok("Service rental has been cancelled successfully.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while cancelling the service rental.");
            }
        }

    }
}
