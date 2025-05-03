using DataAccess;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LandlordLicenseRepository : ILandlordLicenseRepository
    {
        public async Task DeleteLandlordLicenseAsync(LandlordLicense b) => await LandlordLicenseDAO.DeleteLandlordLicenseAsync(b);
        public async Task<LandlordLicense> GetLandlordLicenseByIdAsync(int id) => await LandlordLicenseDAO.FindLandlordLicenseByIdAsync(id);
        public async Task<List<LandlordLicenseDTO>> GetLandlordLicensesAsync() => await LandlordLicenseDAO.GetLandlordLicensesAsync();
        public async Task SaveLandlordLicenseAsync(LandlordLicense b) => await LandlordLicenseDAO.SaveLandlordLicenseAsync(b);
        public async Task UpdateLandlordLicenseAsync(LandlordLicense b) => await LandlordLicenseDAO.UpdateLandlordLicenseAsync(b);
        //public async Task<bool> IsCCCDExistsAsync(string cccd)
        //                => await LandlordLicenseDAO.IsCCCDExistsAsync(cccd);

        public async Task<LandlordLicense?> GetByUserIdAsync(int userId)
            => await LandlordLicenseDAO.GetByUserIdAsync(userId);
        public async Task<bool> IsCCCDExistsAsync(string cccd)
        {
            try
            {
                return await LandlordLicenseDAO.IsCCCDExistsAsync(cccd);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra CCCD: " + ex.Message);
            }
        }

    }
}