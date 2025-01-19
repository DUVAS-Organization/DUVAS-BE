using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DUVAS;
using Repositories.IRepository;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomManagementController : ControllerBase
    {
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IUserFeedbackRepository _userFeedbackRepository;

        public RoomManagementController(IRentalListRepository rentalListRepository, IUserFeedbackRepository userFeedbackRepository)
        {
            _rentalListRepository = rentalListRepository;
            _userFeedbackRepository = userFeedbackRepository;
        }

        /// <summary>
        /// Rent a room
        /// </summary>
        [HttpPost("rent-room")]
        public async Task<IActionResult> RentRoom([FromBody] RentalList rentalRequest)
        {
            if (rentalRequest == null || rentalRequest.RoomId <= 0 || rentalRequest.RenterID <= 0)
            {
                return BadRequest("Invalid rental request.");
            }

            rentalRequest.RentalStatus = 0; // New rental request
            await _rentalListRepository.SaveRentalListAsync(rentalRequest);
            return Ok("Room rental request created successfully.");
        }


        /// <summary>
        /// Cancel room rental
        /// </summary>
        [HttpPut("cancel-room/{rentalId}")]
        public async Task<IActionResult> CancelRoom(int rentalId)
        {
            var rental = await _rentalListRepository.GetRentalListByIdAsync(rentalId);
            if (rental == null)
            {
                return NotFound("Rental not found.");
            }

            rental.RentalStatus = -1; // Set status to 'Cancelled'
            await _rentalListRepository.UpdateRentalListAsync(rental);

            return Ok("Room rental cancelled successfully.");
        }


        /// <summary>
        /// View room rental status
        /// </summary>
        [HttpGet("rental-status/{roomId}")]
        public async Task<IActionResult> ViewRoomRentalStatus(int roomId)
        {
            var rentals = await _rentalListRepository.GetRentalListsAsync();
            var roomRentals = rentals.FindAll(r => r.RoomId == roomId);

            if (roomRentals.Count == 0)
            {
                return NotFound("No rentals found for the specified room.");
            }

            return Ok(roomRentals);
        }


        /// <summary>
        /// View room reviews
        /// </summary>
        [HttpGet("room-reviews/{roomId}")]
        public async Task<IActionResult> ViewRoomReviews(int roomId)
        {
            var feedbacks = await _userFeedbackRepository.GetUserFeedbacksAsync();
            var roomFeedbacks = feedbacks.FindAll(f => f.UserId == roomId); // Assuming feedback is linked to the room via UserId

            if (roomFeedbacks.Count == 0)
            {
                return NotFound("No reviews found for the specified room.");
            }

            return Ok(roomFeedbacks);
        }

    }
}
