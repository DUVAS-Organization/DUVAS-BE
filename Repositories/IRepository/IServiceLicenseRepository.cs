using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IServiceLicenseRepository
    {
        Task SaveServiceLicenseAsync(ServiceLicense b);
        Task<ServiceLicense> GetServiceLicenseByIdAsync(int id);
        Task DeleteServiceLicenseAsync(ServiceLicense b);
        Task UpdateServiceLicenseAsync(ServiceLicense b);
        Task<List<ServiceLicenseDTO>> GetServiceLicensesAsync();
        Task<bool> IsCCCDExistsAsync(string cccd); // Thêm để kiểm tra CCCD
        Task<ServiceLicense?> GetByUserIdAsync(int userId); // Thêm để kiểm tra đơn đang chờ
    }
}
