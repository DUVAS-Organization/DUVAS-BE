using DataAccess;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class LandlordLicenseRepository : ILandlordLicenseRepository
    {
        private readonly LandlordLicenseDAO _dao;

        public LandlordLicenseRepository(LandlordLicenseDAO dao)
        {
            _dao = dao;
        }

        public async Task DeleteLandlordLicenseAsync(LandlordLicense b)
            => await _dao.DeleteLandlordLicenseAsync(b);

        public async Task<LandlordLicense> GetLandlordLicenseByIdAsync(int id)
            => await _dao.FindLandlordLicenseByIdAsync(id);

        public async Task<List<LandlordLicenseDTO>> GetLandlordLicensesAsync()
            => await _dao.GetLandlordLicensesAsync();

        public async Task SaveLandlordLicenseAsync(LandlordLicense b)
            => await _dao.SaveLandlordLicenseAsync(b);

        public async Task UpdateLandlordLicenseAsync(LandlordLicense b)
            => await _dao.UpdateLandlordLicenseAsync(b);

        public async Task<bool> IsCCCDExistsAsync(string cccd)
        {
            try
            {
                return await _dao.IsCCCDExistsAsync(cccd);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra CCCD: {ex.Message}");
            }
        }

        public async Task<LandlordLicense?> GetByUserIdAsync(int userId)
            => await _dao.GetByUserIdAsync(userId);
    }
}