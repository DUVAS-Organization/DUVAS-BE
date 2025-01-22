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
    public class RentalListDAO
    {
        private readonly ApplicationDbContext _context;

        public RentalListDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<RentalListDTO>> GetRentalListsAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var rentals = await context.RentalLists
                        .AsNoTracking()
                        .Select(p => new RentalListDTO
                        {
                            RentalId = p.RentalId,
                            ContractId = p.ContractId,
                            RenterID = p.RenterID,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return rentals;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<RentalList> FindRentalListByIdAsync(int rentalId)
        {
            RentalList rental = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    rental = await context.RentalLists.SingleOrDefaultAsync(x => x.RentalId == rentalId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return rental;
        }

        public static async Task SaveRentalListAsync(RentalList rental)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.RentalLists.AddAsync(rental);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateRentalListAsync(RentalList rental)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(rental).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteRentalListAsync(RentalList rental)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingRentalList = await context.RentalLists.SingleOrDefaultAsync(c => c.RentalId == rental.RentalId);
                    if (existingRentalList != null)
                    {
                        context.RentalLists.Remove(existingRentalList);
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
