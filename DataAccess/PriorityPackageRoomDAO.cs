using BusinessObject;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PriorityPackageRoomDAO
    {
        private readonly ApplicationDbContext _context;

        public PriorityPackageRoomDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả PriorityPackageRoom
        public static async Task<List<PriorityPackageRoomDTO>> GetPriorityPackageRoomsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var packages = await context.PriorityPackageRooms
                        .AsNoTracking()
                        .Select(p => new PriorityPackageRoomDTO
                        {
                            PriorityPackageRoomId = p.PriorityPackageRoomId,
                            UserId = p.UserId,
                            RoomId = p.RoomId,
                            CategoryPriorityPackageRoomId = p.CategoryPriorityPackageRoomId,
                            StartDate = p.StartDate.ToString("HH:mm - dd/MM/yyyy"),
                            EndDate = p.EndDate.ToString("HH:mm - dd/MM/yyyy"),
                            Price = p.Price
                        })
                        .ToListAsync();

                    return packages;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách PriorityPackageRoom: {ex.Message}");
            }
        }

        // Tìm kiếm PriorityPackageRoom theo ID
        public static async Task<PriorityPackageRoomDTO> FindPriorityPackageRoomByIdAsync(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var package = await context.PriorityPackageRooms
                        .Where(p => p.PriorityPackageRoomId == id)
                        .Select(p => new PriorityPackageRoomDTO
                        {
                            PriorityPackageRoomId = p.PriorityPackageRoomId,
                            UserId = p.UserId,
                            RoomId = p.RoomId,
                            CategoryPriorityPackageRoomId = p.CategoryPriorityPackageRoomId,
                            StartDate = p.StartDate.ToString("HH:mm - dd/MM/yyyy"),
                            EndDate = p.EndDate.ToString("HH:mm - dd/MM/yyyy"),
                            Price = p.Price
                        })
                        .FirstOrDefaultAsync();

                    return package ?? throw new Exception("Không tìm thấy PriorityPackageRoom.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm PriorityPackageRoom: {ex.Message}");
            }
        }

        // Thêm PriorityPackageRoom mới
        public static async Task SavePriorityPackageRoomAsync(PriorityPackageRoom package)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var category = await context.CategoryPriorityPackageRooms
                        .Where(c => c.CategoryPriorityPackageRoomId == package.CategoryPriorityPackageRoomId)
                        .Select(c => new { c.CategoryPriorityPackageRoomValue, c.Price })
                        .FirstOrDefaultAsync();

                    if (category == null)
                        throw new Exception("CategoryPriorityPackageRoom không tồn tại.");

                    // Cập nhật giá trị StartDate, EndDate, Price
                    package.StartDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                    package.EndDate = package.StartDate.AddDays(category.CategoryPriorityPackageRoomValue);
                    package.Price = category.Price;

                    await context.PriorityPackageRooms.AddAsync(package);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm PriorityPackageRoom: {ex.Message}");
            }
        }

        // Cập nhật PriorityPackageRoom
        public static async Task UpdatePriorityPackageRoomAsync(PriorityPackageRoom package)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingPackage = await context.PriorityPackageRooms
                        .FirstOrDefaultAsync(p => p.PriorityPackageRoomId == package.PriorityPackageRoomId);

                    if (existingPackage == null)
                        throw new Exception("PriorityPackageRoom không tồn tại.");

                    var category = await context.CategoryPriorityPackageRooms
                        .Where(c => c.CategoryPriorityPackageRoomId == package.CategoryPriorityPackageRoomId)
                        .Select(c => new { c.CategoryPriorityPackageRoomValue, c.Price })
                        .FirstOrDefaultAsync();

                    if (category == null)
                        throw new Exception("CategoryPriorityPackageRoom không tồn tại.");

                    // Cập nhật giá trị EndDate và Price
                    existingPackage.StartDate = package.StartDate;
                    existingPackage.EndDate = package.StartDate.AddDays(category.CategoryPriorityPackageRoomValue);
                    existingPackage.Price = category.Price;

                    context.Entry(existingPackage).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật PriorityPackageRoom: {ex.Message}");
            }
        }

        // Xóa PriorityPackageRoom
        public static async Task DeletePriorityPackageRoomAsync(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingPackage = await context.PriorityPackageRooms
                        .SingleOrDefaultAsync(c => c.PriorityPackageRoomId == id);

                    if (existingPackage == null)
                        throw new Exception("PriorityPackageRoom không tồn tại.");

                    context.PriorityPackageRooms.Remove(existingPackage);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa PriorityPackageRoom: {ex.Message}");
            }
        }
    }
}