using Microsoft.AspNetCore.Mvc;
using API.Utils;
using BusinessObject;
using DataAccess;
using DTO;
using DUVAS;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Core.Types;
using Repositories;
using Repositories.IRepository;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers.Landlord
{
    [Route("api/landlord/[controller]")]
    [ApiController]
    public class OverviewController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRentalListRepository _rentalListRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInsiderTradingRepository _insiderTradingRepository;
        public OverviewController(IInsiderTradingRepository insiderTradingRepository,
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

        [HttpGet()]
        public async Task<IActionResult> GetOverviewOfLandlord(int landlordId)
        {


            // Kiểm tra quyền Landlord
            if (!await IsLandlord(landlordId))
            {
                return Unauthorized(CommonLand.YOU_ARE_NOT_LANLORD);
            }

            // Tổng số phòng của landlord
            var rooms = await _roomRepository.GetRoomsByLandlordAsync(landlordId);
            int totalRooms = rooms.Count;
            int rentedRooms = rooms.Count(r => r.status == 3 & r.IsPermission == 1);
            int availableRooms = rooms.Count(r => r.status == 1 & r.IsPermission == 1);
            int pendingRooms = rooms.Count(r => r.status ==2 & r.IsPermission == 1);
            int lockedRooms = rooms.Count(r => r.IsPermission == 0);

            // phong thuê
            var rentalLists = await _rentalListRepository.GetRentalListsAsync();
            var contracts = await _contractRepository.GetContractsAsync();
            // Lấy danh sách roomIds của landlord
            var roomIds = rooms.Select(r => r.RoomId).ToList();

            // Lấy danh sách rentalLists đã từng đc thuê
            var filteredRentalLists = rentalLists
                .Where(rl =>
                    roomIds.Contains(rl.RoomId) && // RoomId thuộc rooms của landlord
                    rl.ContractId != null &&      // ContractId không null
                    contracts.Any(c => c.ContractId == rl.ContractId && c.Status == 3 && c.Status == 1) // Contract có Status = 1
                )
                .ToList();
            int completedContracts = filteredRentalLists.Count;

            // tổng số rentallist
            var totalRentalLists = rentalLists
                .Where(rl =>
                    roomIds.Contains(rl.RoomId) 
                )
                .ToList();
            int totalRentalList = totalRentalLists.Count;

            // Tổng doanh thu
            var insiderTradings = await _insiderTradingRepository.GetInsiderTradingsAsync();
            var insiderTradingsOfLandlord = insiderTradings.Where(r => r.Receiver == landlordId & r.Status == 1);
            decimal totalInsiderTradingAmount = 0;

            foreach (var trading in insiderTradingsOfLandlord)
            {
                if (trading != null)
                {
                    totalInsiderTradingAmount += trading.Money;
                }
            }

            var overview = new
            {
                TotalRooms = totalRooms,//tổng số room
                RentedRooms = rentedRooms,//số phòng đang đc thuê
                AvailableRooms = availableRooms,//số phòng đang trống
                pendingRooms = pendingRooms,//số phòng đang pending
                LockedRooms = lockedRooms,//số phòng đang bị khóa

                TotalRentalLists = totalRentalList,//số yêu cầu thuê
                CompletedContracts = completedContracts,//số hợp đồng đã đc thuê

                TotalInsiderTradingAmount = totalInsiderTradingAmount,//tổng doanh thu

            };

            return Ok(overview);
        }

    }
}
