using BusinessObject;
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
    public class CategoryPriorityPackageRoomDAO
    {
        private readonly ApplicationDbContext _context;

        public CategoryPriorityPackageRoomDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public static async Task<List<CategoryPriorityPackageRoomDTO>> GetCategoryPriorityPackageRoomsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryPriorityPackageRooms = await context.CategoryPriorityPackageRooms
                        .AsNoTracking()
                        .Select(p => new CategoryPriorityPackageRoomDTO
                        {
                            CategoryPriorityPackageRoomId = p.CategoryPriorityPackageRoomId,
                            CategoryPriorityPackageRoomValue = p.CategoryPriorityPackageRoomValue,
                            Price = p.Price
                        })
                        .ToListAsync();

                    return categoryPriorityPackageRooms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<CategoryPriorityPackageRoom> FindCategoryPriorityPackageRoomByIdAsync(int categoryPriorityPackageRoomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.CategoryPriorityPackageRooms
                        .SingleOrDefaultAsync(x => x.CategoryPriorityPackageRoomId == categoryPriorityPackageRoomId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task SaveCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.CategoryPriorityPackageRooms.AddAsync(categoryPriorityPackageRoom);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(categoryPriorityPackageRoom).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteCategoryPriorityPackageRoomAsync(CategoryPriorityPackageRoom categoryPriorityPackageRoom)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingCategory = await context.CategoryPriorityPackageRooms
                        .SingleOrDefaultAsync(c => c.CategoryPriorityPackageRoomId == categoryPriorityPackageRoom.CategoryPriorityPackageRoomId);
                    if (existingCategory != null)
                    {
                        context.CategoryPriorityPackageRooms.Remove(existingCategory);
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