using API.Utils;
using DataAccess;
using DTO;
using DUVAS;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
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
        private readonly IInsiderTradingRepository _insiderTradingRepository;
        public BookingManagementController(IInsiderTradingRepository insiderTradingRepository, IRoomRepository roomRepository, IRentalListRepository rentalListRepository, IContractRepository contractRepository, IUserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _insiderTradingRepository = insiderTradingRepository;
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

        [HttpGet("rentalList-of-room")]
        public async Task<IActionResult> GetRentalListOfRoom(int RoomId)
        {
            try
            {
                // Kiểm tra xem phòng có tồn tại không
                var room = await _roomRepository.GetRoomByIdAsync(RoomId);
                if (room == null)
                {
                    return NotFound("Phòng không tồn tại.");
                }

                // Truy vấn RentalList với RoomId, RentalStatus = 1 và ContractId = null
                using (var context = new ApplicationDbContext())
                {
                    var rentalLists = await context.RentalLists
                        .AsNoTracking()
                        .Where(r => r.RoomId == RoomId && r.RentalStatus == 1 && r.ContractId == null)
                        .Select(r => new RentalListDTO
                        {
                            RentalId = r.RentalId,
                            ContractId = r.ContractId,
                            RenterID = r.RenterID,
                            RoomId = r.RoomId,
                            RentDate = r.RentDate,
                            MonthForRent = r.MonthForRent,
                            CreatedDate = r.CreatedDate,
                            RenterName = r.User.Name,
                            RenterEmail = r.User.Gmail,
                            RenterPhone = r.User.Phone,
                            RentalStatus = r.RentalStatus
                        })
                        .ToListAsync();

                    if (rentalLists == null || !rentalLists.Any())
                    {
                        return NotFound("Không tìm thấy RentalList nào phù hợp.");
                    }

                    return Ok(rentalLists);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpGet("rentalList-of-landlord")]
        public async Task<IActionResult> GetRentalListOfLandlord(int landlordId)
        {
            var rooms = await _roomRepository.GetRoomsByLandlordAsync(landlordId);
            var rentalLists = await _rentalListRepository.GetRentalListsAsync();

            var roomIds = rooms.Select(r => r.RoomId).ToList();
            var filteredRentals = rentalLists
                .Where(r => roomIds.Contains(r.RoomId))
                .ToList();
            // Tạo danh sách kết quả với thông tin contract status
            var rentalsWithContractStatus = new List<object>();
            foreach (var rental in filteredRentals)
            {
                int? contractStatus = null;
                if (rental.ContractId.HasValue)
                {
                    var contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                    if (contract != null)
                    {
                        contractStatus = contract.status;
                    }
                }

                rentalsWithContractStatus.Add(new
                {
                    rental.RentalId,
                    rental.RoomId,
                    rental.RenterID,
                    rental.RentalStatus,
                    rental.CreatedDate,
                    rental.MonthForRent,
                    rental.RentDate,
                    rental.RenterName,
                    rental.RenterEmail,
                    rental.RenterPhone,
                    ContractId = rental.ContractId,
                    ContractStatus = contractStatus,
                });
            }
            return Ok(filteredRentals);
        }

        [HttpGet("rentalList-of-user")]
        public async Task<IActionResult> GetRentalListOfUser(int userId)
        {
            // Lấy tất cả rental của user
            var rentals = await _rentalListRepository.GetRentalsByUserIdAsync(userId);

            if (rentals == null || !rentals.Any())
            {
                return NotFound("Không tìm thấy RentalList nào cho người dùng này.");
            }

            // Tạo danh sách kết quả với thông tin đầy đủ
            var rentalsWithDetails = new List<object>();
            foreach (var rental in rentals)
            {
                // Lấy thông tin Room (bắt buộc)
                var room = await _roomRepository.GetRoomByIdAsync(rental.RoomId);
                if (room == null)
                {
                    // Ghi log để debug
                    Console.WriteLine($"Room with RoomId {rental.RoomId} not found for RentalId {rental.RentalId}. Skipping...");
                    continue; // Bỏ qua nếu không tìm thấy phòng
                }

                // Lấy thông tin Contract (nếu có)
                Contract contract = null;
                if (rental.ContractId.HasValue)
                {
                    contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                }

                rentalsWithDetails.Add(new
                {
                    rental.RentalId,
                    rental.RoomId,
                    rental.RenterID,
                    rental.RentalStatus,
                    rental.CreatedDate,
                    rental.MonthForRent,
                    rental.RentDate,
                    rental.RenterName,
                    rental.RenterEmail,
                    rental.RenterPhone,
                    ContractId = rental.ContractId,
                    ContractStatus = contract?.status,
                    RoomStatus = room.status,
                    // Thêm thông tin chi tiết của Room
                    RoomDetails = new
                    {
                        room.RoomId,
                        room.Title,
                        room.Price,
                        room.LocationDetail,
                        room.Image,
                        room.status,
                        LandlordId = room.UserId,
                    },
                    // Thêm thông tin chi tiết của Contract (nếu có)
                    ContractDetails = contract != null ? new
                    {
                        contract.ContractId,
                        contract.RentalDateTimeStart,
                        contract.RentalDateTimeEnd,
                        contract.ContractFile,
                        contract.status
                    } : null
                });
            }

            return Ok(new { rentalList = rentalsWithDetails });
        }

        [HttpPut("confirm-reservation/{roomId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> ConfirmReservation(int RentalList, [FromBody] ContractRequestDTO contractDto)
        {
            // 🔹 Check if room exists
            var rentalLists = await _rentalListRepository.GetRentalListByIdAsync(RentalList);
            var room = await _roomRepository.GetRoomByIdAsync(rentalLists.RoomId);
            if (room == null)
            {
                return NotFound("Phòng không tồn tại.");
            }
            //if (room.status != 2)
            //{
            //    return BadRequest("Phòng phải có trạng thái Pending (2) để xác nhận yêu cầu thuê.");
            //}

            // Update room details (Deposit, Price)
            if (contractDto.Deposit != 0)
            {
                room.Deposit = contractDto.Deposit ?? 0;
            }

            if (contractDto.Price != 0)
            {
                room.Price = contractDto.Price ?? 0;
            }

            await _roomRepository.UpdateRoomAsync(room);

            // Create a new contract
            DateTime formattedDate = DateTime.ParseExact(contractDto.RentalDateTimeEnd, "yyyy-MM-dd", null);
            DateTime formattedDatee = DateTime.ParseExact(contractDto.RentalDateTimeStart, "yyyy-MM-dd", null);
            var contract = new Contract
            {
                RentalDateTimeEnd = formattedDate,
                RentalDateTimeStart = formattedDatee,
                ContractFile = contractDto.ContractFile, // Store contract file if available
                status = 4 // Active contract
            };

            var newContractId = await _contractRepository.NewContractAsync(contract);

            // Cập nhật RentalList với ContractId mới


            // Cập nhật ContractId vào RentalList đã tồn tại
            rentalLists.ContractId = newContractId;

            // Lưu lại RentalList đã được cập nhật
            await _rentalListRepository.UpdateRentalListAsync(rentalLists);
            // **🔥 Cập nhật trạng thái phòng thành Pending (2)**
            room.status = 2;
            await _roomRepository.UpdateRoomAsync(room);
            return Ok("Hợp đồng đã được tạo và yêu cầu thuê đã được xác nhận.");
        }

        // 4. Cancel Reservation
        [HttpPut("cancel-reservation/{rentalId}")]
        [Authorize(Roles = "Landlord")]
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

            //if (room.status != 2)
            //{
            //    return BadRequest("Chỉ có thể hủy yêu cầu thuê khi phòng đang ở trạng thái Pending.");
            //}

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
        
        [HttpPost("create-insider-trading")]
        public async Task<IActionResult> CreateInsiderTrading([FromBody] InsiderTradingDTO dto, string type)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var id = await _insiderTradingRepository.NewInsiderTradingAsync(dto, type);
            return CreatedAtAction(nameof(GetInsiderTradingById), new { id }, dto);
        }

        [HttpGet("get-insider-trading-by-id{id}")]
        public async Task<IActionResult> GetInsiderTradingById(int id)
        {
            var result = await _insiderTradingRepository.GetInsiderTradingByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpPost("create-insider-trading-2")]
        public async Task<IActionResult> CreateInsiderTrading2([FromBody] InsiderTradingDTO insiderTradingDTO, [FromQuery] string type)
        {
            try
            {
                await InsiderTradingDAO.SaveInsiderTradingAsync(insiderTradingDTO, type);
                return Ok(new { Message = "Insider trading record created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating insider trading record", Error = ex.Message });
            }
        }

        [HttpPost("create-book-insider-trading")]
        public async Task<IActionResult> CreateFixedInsiderTrading([FromBody] InsiderTradingRequest request)
        {
            try
            {
                var insiderTradingDTO = new InsiderTradingDTO
                {
                    Remitter = request.Remnitter,
                    Receiver = request.Receiver,
                    Money = request.Money,
                    Note = $"User {request.Remnitter} thanh toán {request.Money} tiền phòng đến User {request.Receiver}",
                    Status = 1, // Giá trị cố định
                    Type = "aaa", // Giá trị cố định
                    CreatedDate = DateTime.Now,
                    HoldUntil = 3 // 3 ngày từ hiện tại
                };

                await InsiderTradingDAO.SaveInsiderTradingAsync(insiderTradingDTO, "aaa");
                return Ok(new { Message = "Insider trading record created successfully with fixed values" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating insider trading record", Error = ex.Message });
            }
        }
       
        [HttpPost("first-month-insider-trading")]
        public async Task<IActionResult> FirstMonthInsiderTrading([FromBody] InsiderTradingRequest request)
        {
            try
            {
                var insiderTradingDTO = new InsiderTradingDTO
                {
                    Remitter = request.Remnitter,
                    Receiver = request.Receiver,
                    Money = request.Money,
                    Note = $"User {request.Remnitter} thanh toán {request.Money} tiền phòng đến User {request.Receiver}",

                    Status = 2, // Giá trị cố định
                    Type = "ThanhToanLanDau", // Giá trị cố định
                    CreatedDate = DateTime.Now,
                    HoldUntil = 3 // 3 ngày từ hiện tại
                };

                int insiderTradingId = await InsiderTradingDAO.SaveInsiderTradingAsync(insiderTradingDTO, "ThanhToanLanDau");
                return Ok(new { Message = "Insider trading record created successfully with fixed values" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating insider trading record", Error = ex.Message });
            }
        }

        [HttpPost("schedule-action")]
        public async Task<IActionResult> ScheduleAction([FromBody] DateTime ActionDate, int landlordId, decimal money, int insiderTradingId)
        {
            if (ActionDate == null || ActionDate == default)
            {
                return BadRequest("Invalid request data.");
            }

            // Schedule job to run after 3 days
            BackgroundJob.Schedule(() => ExecuteScheduledAction(ActionDate, landlordId, money, insiderTradingId), TimeSpan.FromDays(3));

            return Ok(new { Message = "Action scheduled successfully" });
        }

        [HttpPost("cancel-scheduled-action")]
        public async Task<IActionResult> CancelScheduledAction([FromBody] DateTime actionDate)
        {
            // Logic to mark this action as canceled in memory/cache
            CacheHelper.SetCanceledAction(actionDate);

            return Ok(new { Message = "Scheduled action canceled successfully." });
        }

        [NonAction]
        public async Task ExecuteScheduledAction(DateTime actionDate, int landlordId, decimal money, int insiderTradingId)
        {
            if (CacheHelper.IsActionCanceled(actionDate))
            {
                return;
            }

            await _insiderTradingRepository.UpdateInsiderTradingStatusAsync(insiderTradingId, 1);
            await _userRepository.UpdateUserMoneyAsync(landlordId, money);
            Console.WriteLine($"Executing scheduled action for {actionDate}...");
        }

    }
    public static class CacheHelper
    {
        private static readonly HashSet<DateTime> CanceledActions = new();

        public static void SetCanceledAction(DateTime actionDate)
        {
            lock (CanceledActions)
            {
                CanceledActions.Add(actionDate);
            }
        }

        public static bool IsActionCanceled(DateTime actionDate)
        {
            lock (CanceledActions)
            {
                return CanceledActions.Contains(actionDate);
            }
        }
    }
}