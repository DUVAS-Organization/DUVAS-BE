using DTO;
using BusinessObject;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class PriorityPackageRoomRepository : IPriorityPackageRoomRepository
    {
        public async Task<List<PriorityPackageRoomDTO>> GetPriorityPackageRoomsAsync()
        {
            return await PriorityPackageRoomDAO.GetPriorityPackageRoomsAsync();
        }

        public async Task<PriorityPackageRoomDTO> FindPriorityPackageRoomByIdAsync(int id)
        {
            return await PriorityPackageRoomDAO.FindPriorityPackageRoomByIdAsync(id);
        }

        public async Task SavePriorityPackageRoomAsync(PriorityPackageRoom package)
        {
            await PriorityPackageRoomDAO.SavePriorityPackageRoomAsync(package);
        }

        public async Task UpdatePriorityPackageRoomAsync(PriorityPackageRoom package)
        {
            await PriorityPackageRoomDAO.UpdatePriorityPackageRoomAsync(package);
        }

        public async Task DeletePriorityPackageRoomAsync(int id)
        {
            await PriorityPackageRoomDAO.DeletePriorityPackageRoomAsync(id);
        }
    }
}
