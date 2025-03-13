using API.Utils;
using DataAccess;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.IRepository;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers.Landlord
{
    [Route("api/landlord/[controller]")]
    [ApiController]
    public class BookingManagementController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        private readonly IRentalListRepository _rentalListRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;

        public BookingManagementController(IRoomRepository roomRepository, IRentalListRepository rentalListRepository, IContractRepository contractRepository, IUserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
        }

        private int GetLandlordId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var landlordId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
            return landlordId;
        }

        private async Task<bool> IsLandlord(int userId)
        {
            var user = await UserDAO.FindUserByIdAsync(userId);
            return user?.RoleLandlord == 1;
        }

        // 1. View Room List
        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            int landlordId = GetLandlordId();

            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized(CommonLand.YOU_ARE_NOT_LANLORD);
            }

            var rooms = await _roomRepository.GetRoomsByLandlordAsync(landlordId);
            return Ok(rooms);
        }

        [HttpPut("confirm-reservation/{roomId}")]
        [Authorize(Roles="Landlord")]
        public async Task<IActionResult> ConfirmReservation(int roomId, [FromBody] ContractRequestDTO contractDto)
        {

            // 🔹 Kiểm tra phòng có tồn tại không
            var room = await _roomRepository.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return NotFound("Phòng không tồn tại.");
            }
            // Cập nhật phòng trong database
            if (contractDto.Deposit != 0)
            {
                room.Deposit = contractDto.Deposit ?? 0;

            }

            if (contractDto.Price != 0)
            {
                room.Price = contractDto.Price ?? 0;

            }


            await _roomRepository.UpdateRoomAsync(room);
            DateTime formattedDate = DateTime.ParseExact(contractDto.RentalDateTimeEnd, "yyyy-MM-dd", null);
            // Tạo hợp đồng mới
            var contract = new Contract
            {
                RentalDateTimeEnd = formattedDate,
                ContractFile = contractDto.ContractFile, // Lưu file hợp đồng (nếu có)
                status = 1 // Trạng thái hợp đồng: 1 (hợp đồng có hiệu lực)
            };

            var newContractId = await _contractRepository.NewContractAsync(contract);

            
            var rental = new RentalList
            {
                RoomId = room.RoomId,
                ContractId = newContractId,
                RenterID = 17 // Lưu file hợp đồng (nếu có)
            };



            await _rentalListRepository.SaveRentalListAsync(rental);

            return Ok("Hợp đồng đã được tạo và yêu cầu thuê đã được xác nhận.");
        }




        //// 3. Track Room Status
        //[HttpGet("rooms/{roomId}/status")]
        //[Authorize]
        //public async Task<IActionResult> TrackRoomStatus(int roomId)
        //{
        //    int landlordId = GetLandlordId();

        //    var room = await _roomRepository.GetRoomByIdForLandlordAsync(roomId, landlordId);
        //    if (room == null)
        //    {
        //        return NotFound("Phòng không tồn tại hoặc không thuộc chủ nhà.");
        //    }

        //    string statusMessage = room.status switch
        //    {
        //        1 => "Phòng này đang trống và sẵn sàng cho thuê.",
        //        2 => "Phòng này đang cho thuê hoặc đang làm hợp đồng.",
        //        3 => "Phòng này đang được thuê.",
        //        _ => "Phòng này không có trạng thái hợp lệ."
        //    };

        //    return Ok(new { RoomId = roomId, Status = room.status, Message = statusMessage });
        //}

        // 4. Cancel Reservation
        [HttpPut("cancel-reservation/{rentalId}")]
        [Authorize(Roles="Landlord")]
        public async Task<IActionResult> CancelReservation(int rentalId)
        {
            // Kiểm tra RentalList có tồn tại không
            var rentalList = await _rentalListRepository.GetRentalListByIdAsync(rentalId);
            if (rentalList == null)
            {
                return NotFound("Không tìm thấy yêu cầu thuê.");
            }

            // Kiểm tra xem phòng có đang ở trạng thái Pending (đang chờ xác nhận) không
            var room = await _roomRepository.GetRoomByIdAsync(rentalList.RoomId);
            if (room == null)
            {
                return NotFound("Phòng không tồn tại.");
            }

            if (room.status != 2)
            {
                return BadRequest("Chỉ có thể hủy yêu cầu thuê khi phòng đang ở trạng thái Pending.");
            }

            // Nếu có hợp đồng, cập nhật trạng thái hợp đồng là bị hủy
            if (rentalList.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetContractByIdAsync(rentalList.ContractId.Value);
                if (contract != null)
                {
                    contract.status = 2; // Hợp đồng bị hủy
                    await _contractRepository.UpdateContractAsync(contract);
                }
            }

            // Cập nhật trạng thái yêu cầu thuê thành 'Đã hủy'
            rentalList.RentalStatus = 2; // Đã hủy
            rentalList.CreatedDate = DateTime.Now; // Lưu ngày hủy

            await _rentalListRepository.UpdateRentalListAsync(rentalList);

            // Cập nhật trạng thái phòng về 'Trống'
            room.status = 1;
            await _roomRepository.UpdateRoomAsync(room);

            return Ok("Yêu cầu thuê phòng đã được hủy, phòng đã được mở lại để cho thuê.");
        }
        [HttpPost("check-balance")]
        [Authorize]
        public async Task<IActionResult> CheckUserBalance([FromBody] CheckBalanceDTO request)
        {
            // 🔹 Lấy thông tin User từ Database
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // 🔹 Kiểm tra số dư
            if (user.Money >= request.Amount)
            {
                return Ok("Bạn đủ tiền.");
            }
            else
            {
                return BadRequest("Bạn không đủ tiền.");
            }
        }

        [HttpPut("update-balance")]
        [Authorize]
        public async Task<IActionResult> UpdateUserBalance([FromBody] UpdateBalanceDTO request)
        {
            try
            {
                await _userRepository.UpdateUserMoneyAsync(request.UserId, request.Amount);
                return Ok("Cập nhật số dư thành công.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi cập nhật số dư: {ex.Message}");
            }
        }


    }
}
