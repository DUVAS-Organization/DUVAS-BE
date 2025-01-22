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
    }
}
