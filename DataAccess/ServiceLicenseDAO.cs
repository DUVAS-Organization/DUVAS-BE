using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ServiceLicenseDAO
    {
        private readonly ApplicationDbContext _context;

        public ServiceLicenseDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<ServiceLicenseDTO>> GetServiceLicensesAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var serviceLicenses = await context.ServiceLicenses
                        .AsNoTracking()
                        .Select(p => new ServiceLicenseDTO
                        {
                            ServiceLicenseId = p.ServiceLicenseId,
                            UserId = p.UserId,
                            ServiceLicense1 = p.ServiceLicense1,
                            ServiceLicense2 = p.ServiceLicense2,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return serviceLicenses;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<ServiceLicense> FindServiceLicenseByIdAsync(int serviceLicenseId)
        {
            ServiceLicense serviceLicense = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    serviceLicense = await context.ServiceLicenses.SingleOrDefaultAsync(x => x.ServiceLicenseId == serviceLicenseId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return serviceLicense;
        }

        public static async Task SaveServiceLicenseAsync(ServiceLicense serviceLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.ServiceLicenses.AddAsync(serviceLicense);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateServiceLicenseAsync(ServiceLicense serviceLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(serviceLicense).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteServiceLicenseAsync(ServiceLicense serviceLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingServiceLicense = await context.ServiceLicenses.SingleOrDefaultAsync(c => c.ServiceLicenseId == serviceLicense.ServiceLicenseId);
                    if (existingServiceLicense != null)
                    {
                        context.ServiceLicenses.Remove(existingServiceLicense);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
