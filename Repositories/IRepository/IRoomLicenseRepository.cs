using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IRoomLicenseRepository
    {
        Task SaveRoomLicenseAsync(RoomLicense b);
        Task<RoomLicense> GetRoomLicenseByIdAsync(int id);
        Task DeleteRoomLicenseAsync(RoomLicense b);
        Task UpdateRoomLicenseAsync(RoomLicense b);
        Task<List<RoomLicenseDTO>> GetRoomLicensesAsync();
    }
}
