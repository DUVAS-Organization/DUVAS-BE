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
    public class RoomLicenseDAO
    {
        private readonly ApplicationDbContext _context;

        public RoomLicenseDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<RoomLicenseDTO>> GetRoomLicensesAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var roomLicenses = await context.RoomLicenses
                        .AsNoTracking()
                        .Select(p => new RoomLicenseDTO
                        {
                            RoomLicenseId = p.RoomLicenseId,
                            RoomId = p.RoomId,
                            RoomLicense1 = p.RoomLicense1,
                            RoomLicense2 = p.RoomLicense2,
                            RoomLicense3 = p.RoomLicense3,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return roomLicenses;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<RoomLicense> FindRoomLicenseByIdAsync(int roomLicenseId)
        {
            RoomLicense roomLicense = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    roomLicense = await context.RoomLicenses.SingleOrDefaultAsync(x => x.RoomLicenseId == roomLicenseId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return roomLicense;
        }

        public static async Task SaveRoomLicenseAsync(RoomLicense roomLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.RoomLicenses.AddAsync(roomLicense);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateRoomLicenseAsync(RoomLicense roomLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(roomLicense).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteRoomLicenseAsync(RoomLicense roomLicense)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingRoomLicense = await context.RoomLicenses.SingleOrDefaultAsync(c => c.RoomLicenseId == roomLicense.RoomLicenseId);
                    if (existingRoomLicense != null)
                    {
                        context.RoomLicenses.Remove(existingRoomLicense);
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
