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
    public class OwnerLicenseRepository : IOwnerLicenseRepository
    {
        public async Task DeleteOwnerLicenseAsync(OwnerLicense b) => await OwnerLicenseDAO.DeleteOwnerLicenseAsync(b);
        public async Task<OwnerLicense> GetOwnerLicenseByIdAsync(int id) => await OwnerLicenseDAO.FindOwnerLicenseByIdAsync(id);
        public async Task<List<OwnerLicenseDTO>> GetOwnerLicensesAsync() => await OwnerLicenseDAO.GetOwnerLicensesAsync();
        public async Task SaveOwnerLicenseAsync(OwnerLicense b) => await OwnerLicenseDAO.SaveOwnerLicenseAsync(b);
        public async Task UpdateOwnerLicenseAsync(OwnerLicense b) => await OwnerLicenseDAO.UpdateOwnerLicenseAsync(b);
    }
}
