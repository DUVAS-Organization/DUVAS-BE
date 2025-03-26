using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using DUVAS;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentRoomController : ControllerBase
    {
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRoomRepository _roomRepository;

        public RentRoomController(IRentalListRepository rentalListRepository, IContractRepository contractRepository, IRoomRepository roomRepository)
        {
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _roomRepository = roomRepository;
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
            }
            return Ok("Contract and associated rental lists updated successfully.");
        }
    }
}