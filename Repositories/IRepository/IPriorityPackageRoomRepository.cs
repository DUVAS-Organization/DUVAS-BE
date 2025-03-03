using DTO;
using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IPriorityPackageRoomRepository
    {
        Task<List<PriorityPackageRoomDTO>> GetPriorityPackageRoomsAsync();
        Task<PriorityPackageRoomDTO> FindPriorityPackageRoomByIdAsync(int id);
        Task SavePriorityPackageRoomAsync(PriorityPackageRoom package);
        Task UpdatePriorityPackageRoomAsync(PriorityPackageRoom package);
        Task DeletePriorityPackageRoomAsync(int id);
    }
}
