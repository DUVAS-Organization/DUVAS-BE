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
    public class ServicePostDAO
    {
        private readonly ApplicationDbContext _context;

        public ServicePostDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<ServicePostDTO>> GetServicePostsAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var servicePosts = await context.ServicePosts
                        .AsNoTracking()
                        .Select(p => new ServicePostDTO
                        {
                            ServicePostId = p.ServicePostId,
                            Title = p.Title,
                            PhoneNumber = p.PhoneNumber,
                            Price = p.Price,
                            Location = p.Location,
                            Description = p.Description,
                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return servicePosts;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<ServicePost> FindServicePostByIdAsync(int servicePostId)
        {
            ServicePost servicePost = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    servicePost = await context.ServicePosts.SingleOrDefaultAsync(x => x.ServicePostId == servicePostId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return servicePost;
        }

        public static async Task SaveServicePostAsync(ServicePost servicePost)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.ServicePosts.AddAsync(servicePost);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateServicePostAsync(ServicePost servicePost)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(servicePost).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteServicePostAsync(ServicePost servicePost)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingServicePost = await context.ServicePosts.SingleOrDefaultAsync(c => c.ServicePostId == servicePost.ServicePostId);
                    if (existingServicePost != null)
                    {
                        context.ServicePosts.Remove(existingServicePost);
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
