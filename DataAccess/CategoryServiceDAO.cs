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
    public class CategoryServiceDAO
    {
        private readonly ApplicationDbContext _context;

        public CategoryServiceDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<CategoryServiceDTO>> GetCategoryServicesAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryServices = await context.CategoryServices
                        .AsNoTracking()
                        .Select(p => new CategoryServiceDTO
                        {
                            CategoryServiceId = p.CategoryServiceId,
                            CategoryServiceName = p.CategoryServiceName,
                            Status = p.Status,
                        })
                        .ToListAsync();

                    return categoryServices;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<CategoryServiceDTO>> GetCategoryLockedServicesAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryServices = await context.CategoryServices
                        .AsNoTracking()
                        .Where(u => u.Status == 0)
                        .Select(p => new CategoryServiceDTO
                        {
                            CategoryServiceId = p.CategoryServiceId,
                            CategoryServiceName = p.CategoryServiceName,
                            Status = p.Status,
                        })
                        .ToListAsync();

                    return categoryServices;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<CategoryServiceDTO>> GetCategoryActiveServicesAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryServices = await context.CategoryServices
                        .AsNoTracking()
                        .Where(u => u.Status == 1)
                        .Select(p => new CategoryServiceDTO
                        {
                            CategoryServiceId = p.CategoryServiceId,
                            CategoryServiceName = p.CategoryServiceName,
                            Status = p.Status,
                        })
                        .ToListAsync();

                    return categoryServices;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<CategoryService> FindCategoryServiceByIdAsync(int categoryServiceId)
        {
            CategoryService categoryServices = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    categoryServices = await context.CategoryServices.SingleOrDefaultAsync(x => x.CategoryServiceId == categoryServiceId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categoryServices;
        }

        public static async Task SaveCategoryServiceAsync(CategoryService categoryServices)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.CategoryServices.AddAsync(categoryServices);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateCategoryServiceAsync(CategoryService categoryServices)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(categoryServices).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteCategoryServiceAsync(CategoryService categoryServices)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingCategory = await context.CategoryServices.SingleOrDefaultAsync(c => c.CategoryServiceId == categoryServices.CategoryServiceId);
                    if (existingCategory != null)
                    {
                        context.CategoryServices.Remove(existingCategory);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task LockCategoryService(int categoryServiceId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryService = await context.CategoryServices.FirstOrDefaultAsync(u => u.CategoryServiceId == categoryServiceId);
                    if (categoryService == null)
                    {
                        throw new KeyNotFoundException($"Service với ID {categoryServiceId} không tồn tại.");
                    }

                    categoryService.Status = 0;
                    context.CategoryServices.Update(categoryService);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi khóa Service: {ex.Message}");
            }
        }
        public static async Task UnLockCategoryService(int categoryServiceId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryService = await context.CategoryServices.FirstOrDefaultAsync(u => u.CategoryServiceId == categoryServiceId);
                    if (categoryService == null)
                    {
                        throw new KeyNotFoundException($"Service với ID {categoryServiceId} không tồn tại.");
                    }

                    categoryService.Status = 1;
                    context.CategoryServices.Update(categoryService);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi mở khóa Service: {ex.Message}");
            }
        }
    }
}
