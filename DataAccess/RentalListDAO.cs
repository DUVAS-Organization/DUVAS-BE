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
                            RoomId = p.RoomId,
                            RentDate = p.RentDate,
                            MonthForRent = p.MonthForRent,
                            CreatedDate = p.CreatedDate,
                            RentalStatus = p.RentalStatus,
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
        // Cập nhật trạng thái hợp đồng (Xác nhận hoặc Hủy hợp đồng)
        public static async Task UpdateRentalListStatusAsync(int rentalListID, int status)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contract = await context.RentalLists.SingleOrDefaultAsync(c => c.RentalId == rentalListID);
                    if (contract != null)
                    {
                        contract.RentalStatus = status; // 1: Đã xác nhận, 2: Đã hủy
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái hợp đồng: " + ex.Message);
            }
        }
        public static async Task<List<RentalListDTO>> GetRentalsByUserIdAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var rentals = await context.RentalLists
                        .AsNoTracking()
                        .Where(r => r.RenterID == userId) // Lọc theo RenterID
                        .Select(r => new RentalListDTO
                        {
                            RentalId = r.RentalId,
                            ContractId = r.ContractId,
                            RenterID = r.RenterID,
                            RoomId = r.RoomId,
                            RentDate = r.RentDate,
                            MonthForRent = r.MonthForRent,
                            CreatedDate = r.CreatedDate,
                            RentalStatus = r.RentalStatus, 
                            // Thêm các thuộc tính khác nếu cần
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
        public static async Task UpdateRentalListContractAsync(int rentalId, int contractId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var rentalList = await context.RentalLists.SingleOrDefaultAsync(r => r.RentalId == rentalId);
                    if (rentalList != null)
                    {
                        rentalList.ContractId = contractId; // Cập nhật ContractId vào RentalList
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hợp đồng cho RentalList: " + ex.Message);
            }
        }
        public static async Task<RentalList> GetRentalListByRoomIdAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    // Use FirstOrDefaultAsync to prevent the error if there are multiple records
                    var rentalList = await context.RentalLists
                        .Where(r => r.RoomId == roomId)  // Filter by roomId
                        .OrderBy(r => r.CreatedDate)     // Optional: Order by creation date or any other criteria
                        .FirstOrDefaultAsync();          // Get the first rental or null if not found

                    return rentalList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy RentalList theo RoomId: " + ex.Message);
            }
        }

    }

}