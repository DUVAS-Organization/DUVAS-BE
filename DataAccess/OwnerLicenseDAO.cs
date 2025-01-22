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
    public class OwnerLicenseDAO
    {
        private readonly ApplicationDbContext _context;

        public OwnerLicenseDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<OwnerLicenseDTO>> GetOwnerLicensesAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var ownerLicenses = await context.OwnerLicenses
                        .AsNoTracking()
                        .Select(p => new OwnerLicenseDTO
                        {
                            OwnerLicenseId = p.OwnerLicenseId,
                            UserId = p.UserId,
                            OwnerLicense1 = p.OwnerLicense1,
                            OwnerLicense2 = p.OwnerLicense2,
                            OwnerLicense3 = p.OwnerLicense3,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return ownerLicenses;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<OwnerLicense> FindOwnerLicenseByIdAsync(int ownerLicenseId)
        {
            OwnerLicense ownerLicense = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    ownerLicense = await context.OwnerLicenses.SingleOrDefaultAsync(x => x.OwnerLicenseId == ownerLicenseId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ownerLicense;
        }

        public static async Task SaveOwnerLicenseAsync(OwnerLicense ownerLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.OwnerLicenses.AddAsync(ownerLicense);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateOwnerLicenseAsync(OwnerLicense ownerLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(ownerLicense).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteOwnerLicenseAsync(OwnerLicense ownerLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingOwnerLicense = await context.OwnerLicenses.SingleOrDefaultAsync(c => c.OwnerLicenseId == ownerLicense.OwnerLicenseId);
                    if (existingOwnerLicense != null)
                    {
                        context.OwnerLicenses.Remove(existingOwnerLicense);
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
