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
    public class PriorityPackageServicePostDAO
    {
        private readonly ApplicationDbContext _context;

        public PriorityPackageServicePostDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả PriorityPackageServicePost
        public static async Task<List<PriorityPackageServicePostDTO>> GetPriorityPackageServicePostsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var packages = await context.PriorityPackageServicePosts
                        .AsNoTracking()
                        .Select(p => new PriorityPackageServicePostDTO
                        {
                            PriorityPackageServicePostId = p.PriorityPackageServicePostId,
                            UserId = p.UserId,
                            ServicePostId = p.ServicePostId,
                            CategoryPriorityPackageServicePostId = p.CategoryPriorityPackageServicePostId,
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
                throw new Exception($"Lỗi khi lấy danh sách PriorityPackageServicePost: {ex.Message}");
            }
        }

        // Tìm kiếm PriorityPackageServicePost theo ID
        public static async Task<PriorityPackageServicePostDTO> FindPriorityPackageServicePostByIdAsync(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var package = await context.PriorityPackageServicePosts
                        .Where(p => p.PriorityPackageServicePostId == id)
                        .Select(p => new PriorityPackageServicePostDTO
                        {
                            PriorityPackageServicePostId = p.PriorityPackageServicePostId,
                            UserId = p.UserId,
                            ServicePostId = p.ServicePostId,
                            CategoryPriorityPackageServicePostId = p.CategoryPriorityPackageServicePostId,
                            StartDate = p.StartDate.ToString("HH:mm - dd/MM/yyyy"),
                            EndDate = p.EndDate.ToString("HH:mm - dd/MM/yyyy"),
                            Price = p.Price
                        })
                        .FirstOrDefaultAsync();

                    return package ?? throw new Exception("Không tìm thấy PriorityPackageServicePost.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm PriorityPackageServicePost: {ex.Message}");
            }
        }

        // Thêm PriorityPackageServicePost mới
        public static async Task SavePriorityPackageServicePostAsync(PriorityPackageServicePost package)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var category = await context.CategoryPriorityPackageServicePosts
                        .Where(c => c.CategoryPriorityPackageServicePostId == package.CategoryPriorityPackageServicePostId)
                        .Select(c => new { c.CategoryPriorityPackageServicePostValue, c.Price })
                        .FirstOrDefaultAsync();

                    if (category == null)
                        throw new Exception("CategoryPriorityPackageServicePost không tồn tại.");

                    // Cập nhật giá trị StartDate, EndDate, Price
                    package.StartDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                    package.EndDate = package.StartDate.AddDays(category.CategoryPriorityPackageServicePostValue);
                    package.Price = category.Price;

                    await context.PriorityPackageServicePosts.AddAsync(package);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm PriorityPackageServicePost: {ex.Message}");
            }
        }

        // Cập nhật PriorityPackageServicePost
        public static async Task UpdatePriorityPackageServicePostAsync(PriorityPackageServicePost package)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingPackage = await context.PriorityPackageServicePosts
                        .FirstOrDefaultAsync(p => p.PriorityPackageServicePostId == package.PriorityPackageServicePostId);

                    if (existingPackage == null)
                        throw new Exception("PriorityPackageServicePost không tồn tại.");

                    var category = await context.CategoryPriorityPackageServicePosts
                        .Where(c => c.CategoryPriorityPackageServicePostId == package.CategoryPriorityPackageServicePostId)
                        .Select(c => new { c.CategoryPriorityPackageServicePostValue, c.Price })
                        .FirstOrDefaultAsync();

                    if (category == null)
                        throw new Exception("CategoryPriorityPackageServicePost không tồn tại.");

                    // Cập nhật giá trị EndDate và Price
                    existingPackage.StartDate = package.StartDate;
                    existingPackage.EndDate = package.StartDate.AddDays(category.CategoryPriorityPackageServicePostValue);
                    existingPackage.Price = category.Price;

                    context.Entry(existingPackage).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật PriorityPackageServicePost: {ex.Message}");
            }
        }

        // Xóa PriorityPackageServicePost
        public static async Task DeletePriorityPackageServicePostAsync(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingPackage = await context.PriorityPackageServicePosts
                        .SingleOrDefaultAsync(c => c.PriorityPackageServicePostId == id);

                    if (existingPackage == null)
                        throw new Exception("PriorityPackageServicePost không tồn tại.");

                    context.PriorityPackageServicePosts.Remove(existingPackage);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa PriorityPackageServicePost: {ex.Message}");
            }
        }
    }
}