using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ILandlordLicenseRepository
    {
        Task SaveLandlordLicenseAsync(LandlordLicense b);
        Task<LandlordLicense> GetLandlordLicenseByIdAsync(int id);
        Task DeleteLandlordLicenseAsync(LandlordLicense b);
        Task UpdateLandlordLicenseAsync(LandlordLicense b);
        Task<List<LandlordLicenseDTO>> GetLandlordLicensesAsync();
        Task<bool> IsCCCDExistsAsync(string cccd); // Thêm để kiểm tra CCCD
        Task<LandlordLicense?> GetByUserIdAsync(int userId); // Thêm để kiểm tra đơn đang chờ
    }
}