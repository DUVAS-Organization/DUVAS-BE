using API.Controllers.Landlord;
using API.Service;
using DataAccess;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyPaymentController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInsiderTradingRepository _insiderTradingRepository;
        private readonly EmailService _emailService;


        public MonthlyPaymentController(EmailService emailService, IInsiderTradingRepository insiderTradingRepository, IRoomRepository roomRepository, IRentalListRepository rentalListRepository, IContractRepository contractRepository, IUserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _rentalListRepository = rentalListRepository;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _insiderTradingRepository = insiderTradingRepository;
            _emailService = emailService;
        }


        [HttpPost("create-insider-trading")]
        public async Task<IActionResult> CreateInsiderTrading([FromBody] InsiderTradingDTO dto, string type)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var id = await _insiderTradingRepository.NewInsiderTradingAsync(dto, type);
            return CreatedAtAction(nameof(BookingManagementController.GetInsiderTradingById), new { id }, dto);
        }

        [HttpPost("check-user-monthly-payment")]
        [Authorize]
        public async Task<IActionResult> CheckUserBalance([FromBody] int UserId)
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;
            var paid = await InsiderTradingDAO.GetInsiderTradingsAsync();

            bool hasPaid = paid.Any(t => t.Remitter == UserId && t.Type == "aaaa" &&
                                         t.CreatedDate.Month == currentMonth &&
                                         t.CreatedDate.Year == currentYear);

            if (hasPaid)
            {
                return Ok(new { Message = "Người dùng đã thanh toán tháng này." });
            }

            var rentals = await RentalListDAO.GetRentalsByUserIdAsync(UserId);
            if (rentals == null || !rentals.Any())
            {
                return NotFound("Người dùng không có RentalList.");
            }

            foreach (var rental in rentals)
            {
                if (rental.ContractId.HasValue)
                {
                    var contract = await _contractRepository.GetContractByIdAsync(rental.ContractId.Value);
                    if (contract != null && contract.status == 1)
                    {
                        var daysRemaining = (contract.RentalDateTimeEnd - DateTime.UtcNow).TotalDays;
                        if (daysRemaining <= 7 && daysRemaining > 0)
                        {
                            var user = await _userRepository.GetUserByIdAsync(UserId);
                            var room = await _roomRepository.GetRoomByIdAsync(rental.RoomId);

                            var khac = (room.Dien ?? 0) + (room.Nuoc ?? 0) + (room.Internet ?? 0) +
                                       (room.Rac ?? 0) + (room.GuiXe ?? 0) + (room.QuanLy ?? 0) + (room.ChiPhiKhac ?? 0);
                            Decimal Deposit = room.Deposit ?? 0;
                            var sendMailDTO = new SendMailMonthlyPaymentDTO
                            {
                                userEmail = user.Gmail,
                                userName = user.Name,
                                roomName = room.Title,
                                address = room.LocationDetail,
                                price = room.Price,
                                deposit = Deposit,
                                ngayBatDau = contract.RentalDateTimeStart,
                                ngayKetThuc = contract.RentalDateTimeEnd,
                                khac = khac
                            };


                            // 🔹 Gửi email thông báo thanh toán
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

                            return Ok(new
                            {
                                Message = "Hợp đồng sắp hết hạn, email đã được gửi.",
                                RentalId = rental.RentalId,
                                ContractId = rental.ContractId,
                                RoomId = rental.RoomId,
                                RentalDateTimeStart = contract.RentalDateTimeStart,
                                RentalDateTimeEnd = contract.RentalDateTimeEnd,
                                Status = contract.status,
                                DaysRemaining = (int)daysRemaining,
                                UserEmail = sendMailDTO.userEmail,
                                UserName = sendMailDTO.userName,
                                RoomName = sendMailDTO.roomName,
                                Address = sendMailDTO.address,
                                Price = sendMailDTO.price,
                                Deposit = sendMailDTO.deposit,
                                AdditionalInfo = sendMailDTO.khac,
                                StartDate = sendMailDTO.ngayBatDau,
                                EndDate = sendMailDTO.ngayKetThuc
                            });
                        }
                    }
                }
            }

            return NotFound("Không có hợp đồng nào sắp hết hạn trong 7 ngày.");
        }





        [HttpPost("send-mail-monthly-payment")]
        //[Authorize]
        public async Task<IActionResult> sendMail([FromBody] SendMailMonthlyPaymentDTO sendMailDTO)
        {
            var landlord = await _userRepository.GetUserByIdAsync(sendMailDTO.userID);

            _emailService.SendMonthlyPaymentToUser(
                sendMailDTO.userEmail,
                sendMailDTO.userName,
                sendMailDTO.roomName,
                sendMailDTO.address,
                sendMailDTO.price,
                sendMailDTO.deposit,
                sendMailDTO.khac,
                sendMailDTO.ngayBatDau,
                sendMailDTO.ngayKetThuc);
            //_emailService.SendRentalNotificationToLandlord(landlord.Gmail!, sendMailDTO.RoomId, sendMailDTO.RenterName);

            return Ok("Gửi mail thành công.");
        }
    }
}