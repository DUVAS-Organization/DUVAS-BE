using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Repositories;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentRoomController : ControllerBase
    {
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserFeedbackRepository _userFeedbackRepository;

        public RentRoomController(IRentalListRepository rentalListRepository,
            IContractRepository contractRepository,
            IRoomRepository roomRepository,
            IUserRepository userRepository,
            IUserFeedbackRepository userFeedbackRepository
            )
        {
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _userFeedbackRepository = userFeedbackRepository;
        }

        // API lấy danh sách RentalList có ContractID tồn tại và Contract có Status = 3
        [HttpGet("rental-lists-with-contract-status-3")]
        public async Task<ActionResult<List<RentalListDTO>>> GetRentalListsWithContractStatus3()
        {
            var rentalLists = await _rentalListRepository.GetRentalListsAsync();
            var filteredList = rentalLists.FindAll(r => r.ContractId.HasValue && r.ContractId > 0);

            var result = new List<RentalListDTO>();
            foreach (var rental in filteredList)
            {
                var contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                if (contract != null && contract.status == 3)
                {
                    result.Add(rental);
                }
            }
            return Ok(result);
        }

        [HttpGet("rental-list-of-user/{userId}")]
        public async Task<IActionResult> GetRentalListsByUserId(int userId)
        {
            var rentals = await _rentalListRepository.GetRentalsByUserIdAsync(userId);
            var filteredRentals = rentals.FindAll(r => r.ContractId.HasValue && r.ContractId != 0);
            var validRentals = new List<RentalListDTO>();

            foreach (var rental in filteredRentals)
            {
                var contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                if (contract != null && contract.status == 4)
                {
                    validRentals.Add(rental);
                }
            }
            return Ok(new { RentalList = validRentals });
        }

        //Đã thuê
        [HttpGet("rental-list-of-rented-user/{userId}")]
        public async Task<IActionResult> GetListsRentedByUserId(int userId)
        {
            var rentals = await _rentalListRepository.GetRentalsByUserIdAsync(userId);
            var filteredRentals = rentals.FindAll(r => r.ContractId.HasValue && r.ContractId != 0);
            var validRentals = new List<RentalListDTO>();

            foreach (var rental in filteredRentals)
            {
                var contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                if (contract != null && contract.status == 3)
                {
                    validRentals.Add(rental);
                }
            }
            return Ok(new { RentalList = validRentals });
        }
        //Đang thuê
        [HttpGet("rental-list-of-rent-user/{userId}")]
        public async Task<IActionResult> GetListsRentingByUserId(int userId)
        {
            var rentals = await _rentalListRepository.GetRentalsByUserIdAsync(userId);
            var filteredRentals = rentals.FindAll(r => r.ContractId.HasValue && r.ContractId != 0);
            var validRentals = new List<RentalListDTO>();

            foreach (var rental in filteredRentals)
            {
                var contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                if (contract != null && contract.status == 1)
                {
                    validRentals.Add(rental);
                }
            }
            return Ok(new { RentalList = validRentals });
        }
        //Đã hủy
        [HttpGet("rental-list-of-cancel-user/{userId}")]
        public async Task<IActionResult> GetListsCancelRentByUserId(int userId)
        {
            var rentals = await _rentalListRepository.GetRentalsByUserIdAsync(userId);
            var filteredRentals = rentals.FindAll(r => r.ContractId.HasValue && r.ContractId != 0);
            var validRentals = new List<RentalListDTO>();

            foreach (var rental in filteredRentals)
            {
                var contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                if (contract != null && contract.status == 2)
                {
                    validRentals.Add(rental);
                }
            }
            return Ok(new { RentalList = validRentals });
        }

        // API lấy chi tiết RentalList và Contract
        [HttpGet("rental-list-by-id/{id}")]
        public async Task<ActionResult<object>> GetRentalListWithContract(int id)
        {
            var rental = await _rentalListRepository.GetRentalListByIdAsync(id);
            if (rental == null) return NotFound("RentalList not found");

            Contract contract = null;
            Room room = null;
            if (rental.ContractId.HasValue)
            {
                contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                room = await _roomRepository.GetRoomByIdAsync(rental.RoomId);
            }

            return Ok(new { RentalList = rental, Contract = contract, Room = room });
        }

        [HttpPut("confirm-rental/{rentId}")]
        public async Task<IActionResult> ConfirmContract(int rentId)
        {
            var rentals = await _rentalListRepository.GetRentalListByIdAsync(rentId);
            if (rentals.ContractId == null)
            {
                return BadRequest("RentalList không tồn tại ContractID");
            }
            else
            {
                int contractId = (int)rentals.ContractId;
                await _contractRepository.UpdateContractStatusAsync(contractId, 1);
                await _rentalListRepository.UpdateRentalListStatusAsync(rentId, 1);
                var rooms = await _roomRepository.GetRoomByIdAsync(rentals.RoomId);
                await _roomRepository.UpdateRoomStatusAsync(rentals.RoomId, rooms.UserId, 3);
            }
            return Ok("Contract and associated rental lists updated successfully.");
        }

        [HttpPut("cancel-rental/{rentId}")]
        public async Task<IActionResult> UpdateContractStatus(int rentId)
        {
            var rentals = await _rentalListRepository.GetRentalListByIdAsync(rentId);
            if (rentals.ContractId == null)
            {
                return BadRequest("RentalList không tồn tại ContractID");
            }
            else
            {
                int contractId = (int)rentals.ContractId;
                await _contractRepository.UpdateContractStatusAsync(contractId, 2);
                await _rentalListRepository.UpdateRentalListStatusAsync(rentId, 2);
                var rooms = await _roomRepository.GetRoomByIdAsync(rentals.RoomId);
                await _roomRepository.UpdateRoomStatusAsync(rentals.RoomId, rooms.UserId, 1);
            }
            return Ok("Contract and associated rental lists updated successfully.");
        }
        [HttpGet("check-phone/{userId}")]
        public async Task<IActionResult> CheckUserPhone(int userId)
        {
            // Lấy thông tin user theo userId
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User không tồn tại.");
            }

            // Kiểm tra số điện thoại: không null, không rỗng và có độ dài tối thiểu (ví dụ: 10 ký tự)
            bool hasValidPhone = !string.IsNullOrEmpty(user.Phone) && user.Phone.Trim().Length >= 10;

            return Ok(new
            {
                UserId = user.UserId,
                HasValidPhone = hasValidPhone,
                Message = hasValidPhone ? "User có số điện thoại hợp lệ." : "User chưa có số điện thoại hợp lệ."
            });
        }
        
        [HttpPost("send-review")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> UserSendReview([FromBody] UserFeedbackDTO review)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("User không được xác thực!");
            }
            
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("UserID không hợp lệ!");
            }

            var rentals = await _rentalListRepository.GetRentalsByUserIdAsync(userId);
            if (rentals.All(r => r.ContractId != review.ContractId))
            {
                return BadRequest("Không có ContractID");
            }
            
            var contract = await _contractRepository.GetContractByIdAsync(review.ContractId.Value);
            if (contract.status != 3 || contract.RentalDateTimeEnd > DateTime.Now)
            {
                Console.WriteLine($"Validation failed: status={contract.status}, endDate={contract.RentalDateTimeEnd}, now={DateTime.Now}");
                return BadRequest("Contract Status hoặc thời gian chấm dứt không hợp lệ!");
            }
            
            review.UserId = userId;

            await _userFeedbackRepository.SaveUserFeedbackAsync(review);
            return Ok("Thành công");
        }

        [HttpGet("get-feedbacks/{roomId}")]
        public async Task<IActionResult> GetUserFeedbackForRoom(int roomId)
        {
            var userFeedbackList = await _userFeedbackRepository.GetUserFeedbacksByRoomIdAsync(roomId);
            return Ok(userFeedbackList);
        }

    }
}