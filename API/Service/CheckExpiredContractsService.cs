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

        // Constructor: Nhận IServiceScopeFactory thay vì các repository trực tiếp
        public CheckExpiredContractsService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Tạo scope mới để truy cập các dịch vụ Scoped
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Lấy các repository từ scope
                        var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
                        var rentalListRepository = scope.ServiceProvider.GetRequiredService<IRentalListRepository>();
                        var contractRepository = scope.ServiceProvider.GetRequiredService<IContractRepository>();

                        // Gọi logic kiểm tra và cập nhật
                        await CheckAndUpdateExpiredContracts(roomRepository, rentalListRepository, contractRepository);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi kiểm tra hợp đồng hết hạn: {ex.Message}");
                }

                // Chờ 24 giờ trước khi kiểm tra lại
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        // Logic kiểm tra và cập nhật hợp đồng hết hạn
        private async Task CheckAndUpdateExpiredContracts(
            IRoomRepository roomRepository,
            IRentalListRepository rentalListRepository,
            IContractRepository contractRepository)
        {
            var currentDate = DateTime.Now;

            var rentalLists = await rentalListRepository.GetRentalListsAsync();
            var validRentalLists = rentalLists.Where(rl => rl.ContractId.HasValue).ToList();

            foreach (var rentalList in validRentalLists)
            {
                var contract = await contractRepository.GetContractByIdAsync(rentalList.ContractId.Value);
                if (contract == null) continue;

                if (contract.RentalDateTimeEnd < currentDate && contract.status != 3)
                {
                    contract.status = 3;
                    await contractRepository.UpdateContractAsync(contract);

                    var room = await roomRepository.GetRoomByIdAsync(rentalList.RoomId);
                    if (room != null && room.status != 1)
                    {
                        room.status = 1;
                        await roomRepository.UpdateRoomAsync(room);
                    }
                }
            }
        }
    }
}