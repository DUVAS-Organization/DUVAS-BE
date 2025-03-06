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
    public class LandlordLicenseDAO
    {
        private readonly ApplicationDbContext _context;

        public LandlordLicenseDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<LandlordLicenseDTO>> GetLandlordLicensesAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var landlordLicenses = await context.LandlordLicenses
                        .AsNoTracking()
                        .Select(p => new LandlordLicenseDTO
                        {
                            LandlordLicenseId = p.LandlordLicenseId,
                            UserId = p.UserId,
                            AnhCCCDMatTruoc = p.AnhCCCDMatTruoc,
                            AnhCCCDMatSau = p.AnhCCCDMatSau,
                            CCCD = p.CCCD,
                            GiayPhepKinhDoanh = p.GiayPhepKinhDoanh,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return landlordLicenses;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<LandlordLicense> FindLandlordLicenseByIdAsync(int landlordLicenseId)
        {
            LandlordLicense landlordLicense = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    landlordLicense = await context.LandlordLicenses.SingleOrDefaultAsync(x => x.LandlordLicenseId == landlordLicenseId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return landlordLicense;
        }

        public static async Task SaveLandlordLicenseAsync(LandlordLicense landlordLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.LandlordLicenses.AddAsync(landlordLicense);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateLandlordLicenseAsync(LandlordLicense landlordLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(landlordLicense).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteLandlordLicenseAsync(LandlordLicense landlordLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingLandlordLicense = await context.LandlordLicenses.SingleOrDefaultAsync(c => c.LandlordLicenseId == landlordLicense.LandlordLicenseId);
                    if (existingLandlordLicense != null)
                    {
                        context.LandlordLicenses.Remove(existingLandlordLicense);
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
