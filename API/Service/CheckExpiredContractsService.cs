using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories.IRepository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public class CheckExpiredContractsService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Kiểm tra mỗi 1 giờ

        public CheckExpiredContractsService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Khởi tạo Timer: chạy lần đầu sau 1 phút, sau đó mỗi 1 giờ
            _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), _checkInterval);
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
                    var rentalListRepository = scope.ServiceProvider.GetRequiredService<IRentalListRepository>();
                    var contractRepository = scope.ServiceProvider.GetRequiredService<IContractRepository>();

                    await CheckAndUpdateExpiredContracts(roomRepository, rentalListRepository, contractRepository);
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (có thể thay Console.WriteLine bằng ILogger nếu bạn đã cấu hình)
                Console.WriteLine($"Lỗi khi kiểm tra hợp đồng hết hạn: {ex.Message}");
            }
        }

        private async Task CheckAndUpdateExpiredContracts(
            IRoomRepository roomRepository,
            IRentalListRepository rentalListRepository,
            IContractRepository contractRepository)
        {
            var currentDate = DateTime.UtcNow; // Sử dụng UTC để đồng bộ múi giờ

            // Lấy tất cả rental lists
            var rentalLists = await rentalListRepository.GetRentalListsAsync();
            var validRentalLists = rentalLists.Where(rl => rl.ContractId.HasValue).ToList();

            foreach (var rentalList in validRentalLists)
            {
                var contract = await contractRepository.GetContractByIdAsync(rentalList.ContractId.Value);
                if (contract == null) continue;

                // Kiểm tra nếu hợp đồng hết hạn và chưa được đánh dấu hết hạn
                if (contract.RentalDateTimeEnd < currentDate && contract.status != 3)
                {
                    contract.status = 3; // Hết hạn
                    await contractRepository.UpdateContractAsync(contract);

                    var room = await roomRepository.GetRoomByIdAsync(rentalList.RoomId);
                    if (room != null && room.status != 1)
                    {
                        room.status = 1; // Phòng trống
                        await roomRepository.UpdateRoomAsync(room);
                    }
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0); // Dừng Timer khi dịch vụ dừng
            _timer?.Dispose(); // Giải phóng tài nguyên
            return base.StopAsync(cancellationToken);
        }
    }
}