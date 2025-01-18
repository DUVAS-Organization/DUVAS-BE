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
    public class RentalServiceListDAO
    {
        private readonly ApplicationDbContext _context;

        public RentalServiceListDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<RentalServiceListDTO>> GetRentalServiceListsAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var rentalServices = await context.RentalServiceLists
                        .AsNoTracking()
                        .Select(p => new RentalServiceListDTO
                        {
                            RentalServiceId = p.RentalServiceId,
                            RenterServiceID = p.RenterID,
                            CreationDateTime = p.CreationDateTime,
                            RentalDateTime = p.RentalDateTime,
                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return rentalServices;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<RentalServiceList> FindRentalServiceListByIdAsync(int rentalServiceId)
        {
            RentalServiceList rentalService = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    rentalService = await context.RentalServiceLists.SingleOrDefaultAsync(x => x.RentalServiceId == rentalServiceId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return rentalService;
        }

        public static async Task SaveRentalServiceListAsync(RentalServiceList rentalService)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.RentalServiceLists.AddAsync(rentalService);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateRentalServiceListAsync(RentalServiceList rentalService)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(rentalService).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteRentalServiceListAsync(RentalServiceList rentalService)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingRentalServiceList = await context.RentalServiceLists.SingleOrDefaultAsync(c => c.RentalServiceId == rentalService.RentalServiceId);
                    if (existingRentalServiceList != null)
                    {
                        context.RentalServiceLists.Remove(existingRentalServiceList);
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
