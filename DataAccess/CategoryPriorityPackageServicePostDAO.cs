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
    public class CategoryPriorityPackageServicePostDAO
    {
        private readonly ApplicationDbContext _context;

        public CategoryPriorityPackageServicePostDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public static async Task<List<CategoryPriorityPackageServicePostDTO>> GetCategoryPriorityPackageServicePostsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categoryPriorityPackageServicePosts = await context.CategoryPriorityPackageServicePosts
                        .AsNoTracking()
                        .Select(p => new CategoryPriorityPackageServicePostDTO
                        {
                            CategoryPriorityPackageServicePostId = p.CategoryPriorityPackageServicePostId,
                            CategoryPriorityPackageServicePostValue = p.CategoryPriorityPackageServicePostValue,
                            Price = p.Price
                        })
                        .ToListAsync();

                    return categoryPriorityPackageServicePosts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<CategoryPriorityPackageServicePost> FindCategoryPriorityPackageServicePostByIdAsync(int categoryPriorityPackageServicePostId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.CategoryPriorityPackageServicePosts
                        .SingleOrDefaultAsync(x => x.CategoryPriorityPackageServicePostId == categoryPriorityPackageServicePostId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task SaveCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.CategoryPriorityPackageServicePosts.AddAsync(categoryPriorityPackageServicePost);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(categoryPriorityPackageServicePost).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteCategoryPriorityPackageServicePostAsync(CategoryPriorityPackageServicePost categoryPriorityPackageServicePost)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingCategory = await context.CategoryPriorityPackageServicePosts
                        .SingleOrDefaultAsync(c => c.CategoryPriorityPackageServicePostId == categoryPriorityPackageServicePost.CategoryPriorityPackageServicePostId);
                    if (existingCategory != null)
                    {
                        context.CategoryPriorityPackageServicePosts.Remove(existingCategory);
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
