using DataAccess;
using DTO;
using DUVAS;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RoomLicenseRepository : IRoomLicenseRepository
    {
        public async Task DeleteRoomLicenseAsync(RoomLicense b) => await RoomLicenseDAO.DeleteRoomLicenseAsync(b);
        public async Task<RoomLicense> GetRoomLicenseByIdAsync(int id) => await RoomLicenseDAO.FindRoomLicenseByIdAsync(id);
        public async Task<List<RoomLicenseDTO>> GetRoomLicensesAsync() => await RoomLicenseDAO.GetRoomLicensesAsync();
        public async Task SaveRoomLicenseAsync(RoomLicense b) => await RoomLicenseDAO.SaveRoomLicenseAsync(b);
        public async Task UpdateRoomLicenseAsync(RoomLicense b) => await RoomLicenseDAO.UpdateRoomLicenseAsync(b);
       
    }
}
