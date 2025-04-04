using API.Service;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu người dùng phải đăng nhập để truy cập các API trong controller này
    public class MonthlyPaymentController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInsiderTradingRepository _insiderTradingRepository;
        private readonly EmailService _emailService;

        // Constructor: Khởi tạo các dependency cần thiết thông qua Dependency Injection
        public MonthlyPaymentController(
            EmailService emailService,
            IInsiderTradingRepository insiderTradingRepository,
            IRoomRepository roomRepository,
            IRentalListRepository rentalListRepository,
            IContractRepository contractRepository,
            IUserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _insiderTradingRepository = insiderTradingRepository;
            _emailService = emailService;
        }

        // API 1a: Lấy danh sách các phòng đang được thuê của Landlord
        [HttpGet("landlord/rented-rooms")]
        [Authorize] // Chỉ Landlord được truy cập
        public async Task<IActionResult> GetRentedRoomsForLandlord()
        {
            try
            {
                // Lấy UserId từ token của người dùng hiện tại (Landlord)
                int landlordId = int.Parse(User.FindFirst("UserId")?.Value ?? throw new Exception("Không tìm thấy UserId trong token"));

                // Lấy danh sách phòng của Landlord có status = 3 (đang được thuê)
                var rooms = await _roomRepository.GetRoomsByLandlordAsync(landlordId);
                var rentedRooms = rooms.Where(r => r.status == 3).ToList();

                var currentDate = DateTime.Now; // Lấy ngày hiện tại
                var insiderTradings = await _insiderTradingRepository.GetInsiderTradingsAsync(); // Lấy tất cả giao dịch

                var result = new List<object>();
                foreach (var room in rentedRooms)
                {
                    // Lấy thông tin rental list của phòng
                    var rentalList = await _rentalListRepository.GetRentalListByRoomIdAsync(room.RoomId);
                    // Lấy hợp đồng liên quan nếu có
                    var contract = rentalList?.ContractId.HasValue == true
                        ? await _contractRepository.GetContractByIdAsync(rentalList.ContractId.Value)
                        : null;

                    if (contract == null) continue; // Bỏ qua nếu không có hợp đồng

                    // Tính ngày đến hạn thanh toán của tháng hiện tại
                    var paymentDueDate = new DateTime(currentDate.Year, currentDate.Month, contract.RentalDateTimeStart.Day);
                    if (currentDate < paymentDueDate) paymentDueDate = paymentDueDate.AddMonths(-1);

                    // Kiểm tra xem phòng đã được thanh toán tháng này chưa
                    var isPaid = insiderTradings.Any(it =>
                        it.RoomId == room.RoomId &&
                        it.CreatedDate.Year == paymentDueDate.Year &&
                        it.CreatedDate.Month == paymentDueDate.Month &&
                        it.Type == "MonthlyPayment" &&
                        it.Status == 1);

                    // Thêm thông tin phòng vào kết quả trả về
                    result.Add(new
                    {
                        RoomId = room.RoomId,
                        Title = room.Title,
                        IsPaidThisMonth = isPaid,
                        PaymentDueDate = paymentDueDate.ToString("dd/MM/yyyy"),
                        RenterId = rentalList?.RenterID
                    });
                }

                return Ok(result); // Trả về danh sách phòng dưới dạng JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}"); // Trả về lỗi nếu có exception
            }
        }

        // API 1b: Lấy danh sách các phòng đang thuê của Renter
        [HttpGet("renter/rented-rooms")]
        [Authorize] // Chỉ Renter được truy cập
        public async Task<IActionResult> GetRentedRoomsForRenter()
        {
            try
            {
                // Lấy UserId từ token của người dùng hiện tại (Renter)
                int renterId = int.Parse(User.FindFirst("UserId")?.Value ?? throw new Exception("Không tìm thấy UserId trong token"));

                // Lấy danh sách rental list mà người dùng là Renter
                var rentalLists = await _rentalListRepository.GetRentalsByUserIdAsync(renterId);
                // Lọc các rental list có ContractId không null
                var validRentalLists = rentalLists.Where(rl => rl.ContractId.HasValue).ToList();

                var currentDate = DateTime.Now; // Lấy ngày hiện tại
                var insiderTradings = await _insiderTradingRepository.GetInsiderTradingsAsync(); // Lấy tất cả giao dịch

                var result = new List<object>();
                foreach (var rentalList in validRentalLists)
                {
                    // Lấy hợp đồng liên quan
                    var contract = await _contractRepository.GetContractByIdAsync(rentalList.ContractId.Value);
                    // Chỉ lấy hợp đồng có status = 1 (đã xác nhận)
                    if (contract == null || contract.status != 1) continue;

                    // Lấy thông tin phòng
                    var room = await _roomRepository.GetRoomByIdAsync(rentalList.RoomId);
                    if (room == null) continue;

                    // Tính ngày đến hạn thanh toán của tháng hiện tại
                    var paymentDueDate = new DateTime(currentDate.Year, currentDate.Month, contract.RentalDateTimeStart.Day);
                    if (currentDate < paymentDueDate) paymentDueDate = paymentDueDate.AddMonths(-1);

                    // Kiểm tra xem phòng đã được thanh toán tháng này chưa
                    var isPaid = insiderTradings.Any(it =>
                        it.RoomId == room.RoomId &&
                        it.CreatedDate.Year == paymentDueDate.Year &&
                        it.CreatedDate.Month == paymentDueDate.Month &&
                        it.Type == "MonthlyPayment" &&
                        it.Status == 1);

                    // Thêm thông tin phòng vào kết quả trả về
                    result.Add(new
                    {
                        RoomId = room.RoomId,
                        Title = room.Title,
                        IsPaidThisMonth = isPaid,
                        PaymentDueDate = paymentDueDate.ToString("dd/MM/yyyy"),
                        LandlordId = room.UserId
                    });
                }

                return Ok(result); // Trả về danh sách phòng dưới dạng JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}"); // Trả về lỗi nếu có exception
            }
        }

        // API 2: Lấy chi tiết thông tin của một phòng cụ thể
        [HttpGet("room-details/{roomId}")]
        public async Task<IActionResult> GetRoomDetails(int roomId)
        {
            try
            {
                // Lấy UserId từ token của người dùng hiện tại
                int userId = int.Parse(User.FindFirst("UserId")?.Value ?? throw new Exception("Không tìm thấy UserId trong token"));

                // Tìm phòng theo RoomId
                var room = await _roomRepository.GetRoomByIdAsync(roomId);
                if (room == null) return NotFound("Phòng không tồn tại");

                // Lấy thông tin rental list của phòng
                var rentalList = await _rentalListRepository.GetRentalListByRoomIdAsync(roomId);
                // Kiểm tra quyền truy cập: Chỉ landlord hoặc renter của phòng được xem
                if (room.UserId != userId && rentalList?.RenterID != userId)
                    return Unauthorized("Bạn không có quyền xem chi tiết phòng này");

                // Lấy hợp đồng liên quan nếu có
                var contract = rentalList?.ContractId.HasValue == true
                    ? await _contractRepository.GetContractByIdAsync(rentalList.ContractId.Value)
                    : null;

                var currentDate = DateTime.Now; // Lấy ngày hiện tại
                // Tính ngày đến hạn thanh toán của tháng hiện tại
                var paymentDueDate = contract != null
                    ? new DateTime(currentDate.Year, currentDate.Month, contract.RentalDateTimeStart.Day)
                    : DateTime.MinValue;
                if (currentDate < paymentDueDate) paymentDueDate = paymentDueDate.AddMonths(-1);

                // Lấy danh sách giao dịch và kiểm tra trạng thái thanh toán tháng này
                var insiderTradings = await _insiderTradingRepository.GetInsiderTradingsAsync();
                var isPaid = insiderTradings.Any(it =>
                    it.RoomId == roomId &&
                    it.CreatedDate.Year == paymentDueDate.Year &&
                    it.CreatedDate.Month == paymentDueDate.Month &&
                    it.Type == "MonthlyPayment" &&
                    it.Status == 1);

                // Tính chi phí khác (điện, nước, internet, rác, gửi xe, quản lý, chi phí khác)
                var khac = (room.Dien ?? 0) + (room.Nuoc ?? 0) + (room.Internet ?? 0) +
                          (room.Rac ?? 0) + (room.GuiXe ?? 0) + (room.QuanLy ?? 0) + (room.ChiPhiKhac ?? 0);

                // Trả về thông tin chi tiết của phòng
                return Ok(new
                {
                    RoomId = room.RoomId,
                    Title = room.Title,
                    LocationDetail = room.LocationDetail,
                    Price = room.Price,
                    Deposit = room.Deposit ?? 0,
                    AdditionalCosts = khac,
                    IsPaidThisMonth = isPaid,
                    PaymentDueDate = paymentDueDate.ToString("dd/MM/yyyy"),//ngày đến hạn thanh toán
                    ContractStart = contract?.RentalDateTimeStart.ToString("dd/MM/yyyy"),
                    ContractEnd = contract?.RentalDateTimeEnd.ToString("dd/MM/yyyy"),
                    RenterId = rentalList?.RenterID,
                    LandlordId = room.UserId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}"); // Trả về lỗi nếu có exception
            }
        }

        // API 3: Tạo yêu cầu thanh toán tiền phòng hàng tháng
        [HttpPost("request-payment/{roomId}")]
        public async Task<IActionResult> RequestMonthlyPayment(int roomId, [FromBody] MonthlyPaymontRequestDTO requestDTO)
        {
            try
            {
                // Lấy UserId từ token của người dùng hiện tại
                int userId = int.Parse(User.FindFirst("UserId")?.Value ?? throw new Exception("Không tìm thấy UserId trong token"));

                // Tìm phòng theo RoomId
                var room = await _roomRepository.GetRoomByIdAsync(roomId);
                if (room == null) return NotFound("Phòng không tồn tại");

                // Kiểm tra quyền: Chỉ landlord (chủ nhà) được tạo yêu cầu thanh toán
                if (room.UserId != userId) return Unauthorized("Chỉ chủ nhà mới có thể tạo yêu cầu thanh toán");

                // Lấy thông tin rental list của phòng
                var rentalList = await _rentalListRepository.GetRentalListByRoomIdAsync(roomId);
                if (rentalList == null || !rentalList.ContractId.HasValue)
                    return BadRequest("Phòng chưa có hợp đồng thuê");

                // Lấy hợp đồng liên quan
                var contract = await _contractRepository.GetContractByIdAsync(rentalList.ContractId.Value);
                if (contract == null) return BadRequest("Không tìm thấy hợp đồng");

                // Lấy thông tin người thuê (renter)
                var renter = await _userRepository.GetUserByIdAsync(rentalList.RenterID);
                if (renter == null) return BadRequest("Không tìm thấy người thuê");

                var currentDate = DateTime.Now; // Lấy ngày hiện tại
                // Tính ngày đến hạn thanh toán của tháng hiện tại
                var paymentDueDate = new DateTime(currentDate.Year, currentDate.Month, contract.RentalDateTimeStart.Day);
                if (currentDate < paymentDueDate) paymentDueDate = paymentDueDate.AddMonths(-1);

                // Kiểm tra xem phòng đã được thanh toán tháng này chưa
                var insiderTradings = await _insiderTradingRepository.GetInsiderTradingsAsync();
                if (insiderTradings.Any(it =>
                    it.RoomId == roomId &&
                    it.CreatedDate.Year == paymentDueDate.Year &&
                    it.CreatedDate.Month == paymentDueDate.Month &&
                    it.Type == "MonthlyPayment" &&
                    it.Status == 1))
                {
                    return BadRequest("Phòng đã được thanh toán tháng này");
                }

                // Tính chi phí khác dựa trên dữ liệu sử dụng từ requestDTO
                var khac = ((room.Dien ?? 0) * (requestDTO.Dien ?? 0)) +
                           ((room.Nuoc ?? 0) * (requestDTO.Nuoc ?? 0)) +
                           ((room.Internet ?? 0) * (requestDTO.Internet ?? 0)) +
                           ((room.Rac ?? 0) * (requestDTO.Rac ?? 0)) +
                           ((room.GuiXe ?? 0) * (requestDTO.GuiXe ?? 0)) +
                           ((room.QuanLy ?? 0) * (requestDTO.QuanLy ?? 0)) +
                           ((room.ChiPhiKhac ?? 0) * (requestDTO.ChiPhiKhac ?? 0));
                decimal deposit = 0; // Không sử dụng deposit trong thanh toán tháng

                // Tạo DTO để gửi email thông báo
                var sendMailDTO = new SendMailMonthlyPaymentDTO
                {
                    userEmail = renter.Gmail,
                    userName = renter.Name,
                    roomName = room.Title,
                    address = room.LocationDetail,
                    price = room.Price,
                    deposit = deposit,
                    ngayBatDau = contract.RentalDateTimeStart,
                    ngayKetThuc = contract.RentalDateTimeEnd,
                    khac = khac
                };

                // Tạo yêu cầu thanh toán trong InsiderTrading
                var paymentRequest = new InsiderTradingDTO
                {
                    Remitter = rentalList.RenterID, // Người gửi (renter)
                    Receiver = room.UserId, // Người nhận (landlord)
                    Money = room.Price + khac, // Tổng tiền cần thanh toán
                    Note = $"Yêu cầu thanh toán tiền phòng tháng {paymentDueDate:MM/yyyy}",
                    RoomId = roomId,
                    Status = 0, // Chưa thanh toán
                    Type = "MonthlyPayment",
                    CreatedDate = DateTime.Now,
                    HoldUntil = 0 // Hạn chót là 5 ngày sau ngày đến hạn
                };

                // Lưu yêu cầu thanh toán vào database
                await _insiderTradingRepository.SaveInsiderTradingAsync(paymentRequest, "MonthlyPayment");

                // Gửi email thông báo cho renter
                _emailService.SendMonthlyPaymentToUser(
                    sendMailDTO.userEmail,
                    sendMailDTO.userName,
                    sendMailDTO.roomName,
                    sendMailDTO.address,
                    sendMailDTO.price,
                    sendMailDTO.deposit,
                    sendMailDTO.khac,
                    sendMailDTO.ngayBatDau,
                    sendMailDTO.ngayKetThuc
                );

                // Trả về thông báo thành công
                return Ok(new { Message = "Yêu cầu thanh toán đã được tạo và email đã được gửi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}"); // Trả về lỗi nếu có exception
            }
        }


    }
}