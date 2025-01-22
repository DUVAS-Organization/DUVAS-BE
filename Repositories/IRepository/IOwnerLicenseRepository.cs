using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IOwnerLicenseRepository
    {
        Task SaveOwnerLicenseAsync(OwnerLicense b);
        Task<OwnerLicense> GetOwnerLicenseByIdAsync(int id);
        Task DeleteOwnerLicenseAsync(OwnerLicense b);
        Task UpdateOwnerLicenseAsync(OwnerLicense b);
        Task<List<OwnerLicenseDTO>> GetOwnerLicensesAsync();
    }
}
