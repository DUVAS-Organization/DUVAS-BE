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
    public class CategoryRoomDAO
    {
        private readonly ApplicationDbContext _context;

        public CategoryRoomDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<CategoryRoomDTO>> GetCategoryRoomsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryRooms = await context.CategoryRooms
                        .AsNoTracking()
                        .Select(p => new CategoryRoomDTO
                        {
                            CategoryRoomId = p.CategoryRoomId,
                            CategoryName = p.CategoryName,
                        })
                        .ToListAsync();

                    return categoryRooms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static async Task<CategoryRoom> FindCategoryRoomByIdAsync(int categoryRoomId)
        {
            CategoryRoom categoryRoom = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    categoryRoom = await context.CategoryRooms.SingleOrDefaultAsync(x => x.CategoryRoomId == categoryRoomId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categoryRoom;
        }

        public static async Task SaveCategoryRoomAsync(CategoryRoom categoryRoom)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.CategoryRooms.AddAsync(categoryRoom);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateCategoryRoomAsync(CategoryRoom categoryRoom)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(categoryRoom).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteCategoryRoomAsync(CategoryRoom categoryRoom)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingCategory = await context.CategoryRooms.SingleOrDefaultAsync(c => c.CategoryRoomId == categoryRoom.CategoryRoomId);
                    if (existingCategory != null)
                    {
                        context.CategoryRooms.Remove(existingCategory);
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
