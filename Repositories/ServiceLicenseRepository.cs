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
    public class ServiceLicenseRepository : IServiceLicenseRepository
    {
        private readonly ServiceLicenseDAO _dao;

        public ServiceLicenseRepository(ServiceLicenseDAO dao)
        {
            _dao = dao;
        }

        public async Task DeleteServiceLicenseAsync(ServiceLicense b)
            => await _dao.DeleteServiceLicenseAsync(b);

        public async Task<ServiceLicense> GetServiceLicenseByIdAsync(int id)
            => await _dao.FindServiceLicenseByIdAsync(id);

        public async Task<List<ServiceLicenseDTO>> GetServiceLicensesAsync()
            => await _dao.GetServiceLicensesAsync();

        public async Task SaveServiceLicenseAsync(ServiceLicense b)
            => await _dao.SaveServiceLicenseAsync(b);

        public async Task UpdateServiceLicenseAsync(ServiceLicense b)
            => await _dao.UpdateServiceLicenseAsync(b);

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

        public async Task<ServiceLicense?> GetByUserIdAsync(int userId)
            => await _dao.GetByUserIdAsync(userId);
    }
}