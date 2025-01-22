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
    public class ServiceLicenseRepository : IServiceLicenseRepository
    {
        public async Task DeleteServiceLicenseAsync(ServiceLicense b) => await ServiceLicenseDAO.DeleteServiceLicenseAsync(b);
        public async Task<ServiceLicense> GetServiceLicenseByIdAsync(int id) => await ServiceLicenseDAO.FindServiceLicenseByIdAsync(id);
        public async Task<List<ServiceLicenseDTO>> GetServiceLicensesAsync() => await ServiceLicenseDAO.GetServiceLicensesAsync();
        public async Task SaveServiceLicenseAsync(ServiceLicense b) => await ServiceLicenseDAO.SaveServiceLicenseAsync(b);
        public async Task UpdateServiceLicenseAsync(ServiceLicense b) => await ServiceLicenseDAO.UpdateServiceLicenseAsync(b);
    }
}
