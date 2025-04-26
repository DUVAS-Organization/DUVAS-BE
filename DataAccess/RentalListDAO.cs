using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                            RenterName = p.User.Name,
                            RenterEmail = p.User.Gmail,
                            RenterPhone = p.User.Phone,
                            RentalStatus = p.RentalStatus,
                            ContractStatus = p.Contract.status,
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

        public static async Task UpdateRentalListStatusAsync(int rentalListID, int status)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contract = await context.RentalLists.SingleOrDefaultAsync(c => c.RentalId == rentalListID);
                    if (contract != null)
                    {
                        contract.RentalStatus = status; // 1: Pending, 2: Cancelled, 3: Confirmed
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
                        .Include(r => r.Contract)
                        .AsNoTracking()
                        .Where(r => r.RenterID == userId)
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
                            RenterName = r.User.Name,
                            RenterEmail = r.User.Gmail,
                            RenterPhone = r.User.Phone,
                            ContractStatus = r.Contract != null ? r.Contract.status : (int?)null
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
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.RentalLists.SingleOrDefaultAsync(x => x.RentalId == rentalId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
                        rentalList.ContractId = contractId;
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
                    var rentalList = await context.RentalLists
                        .Where(r => r.RoomId == roomId)
                        .OrderBy(r => r.CreatedDate)
                        .FirstOrDefaultAsync();
                    return rentalList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy RentalList theo RoomId: " + ex.Message);
            }
        }

        public static async Task<RentalList> GetRentalListByRoomIdAndRenterIdAsync(int roomId, int renterId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.RentalLists
                        .FirstOrDefaultAsync(rl => rl.RoomId == roomId && rl.RenterID == renterId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy RentalList theo RoomId và RenterID: " + ex.Message);
            }
        }

        public static async Task<List<RentalList>> GetPendingRentalListsByRoomIdAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.RentalLists
                        .Where(rl => rl.RoomId == roomId && rl.RentalStatus == 1) // Giả sử 1 là Pending
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách RentalList Pending theo RoomId: " + ex.Message);
            }
        }

        public static async Task<List<RentalList>> GetConfirmedRentalListsByRoomIdAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.RentalLists
                        .Where(rl => rl.RoomId == roomId && rl.RentalStatus == 3) // Giả sử 3 là Confirmed
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách RentalList Confirmed theo RoomId: " + ex.Message);
            }
        }
    }
}