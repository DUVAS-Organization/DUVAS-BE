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
                            Name = p.User.Name,
                            Image = p.Image,
                            UserId = p.UserId,
                            IsPermission = p.IsPermission,
                            CategoryServiceId = p.CategoryServiceId,
                            CategoryServiceName = p.CategoryService.CategoryServiceName

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

        public static async Task<List<ServicePostDTO>> GetListServicePostLockAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var lockedServicePosts = await context.ServicePosts
                        .AsNoTracking()
                        .Where(p => p.IsPermission == 0)
                        .Select(p => new ServicePostDTO
                        {
                            ServicePostId = p.ServicePostId,
                            Title = p.Title,
                            PhoneNumber = p.PhoneNumber,
                            Price = p.Price,
                            Location = p.Location,
                            Description = p.Description,
                            Name = p.User.Name,
                            UserId = p.UserId,
                            IsPermission = p.IsPermission,
                            CategoryServiceName = p.CategoryService.CategoryServiceName
                        })
                        .ToListAsync();

                    return lockedServicePosts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách ServicePost bị khóa: {ex.Message}");
            }
        }
        public static async Task<List<ServicePostDTO>> GetListServicePostActiveAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var lockedServicePosts = await context.ServicePosts
                        .AsNoTracking()
                        .Where(p => p.IsPermission == 0)
                        .Select(p => new ServicePostDTO
                        {
                            ServicePostId = p.ServicePostId,
                            Title = p.Title,
                            PhoneNumber = p.PhoneNumber,
                            Price = p.Price,
                            Location = p.Location,
                            Description = p.Description,
                            Name = p.User.Name,
                            UserId = p.UserId,
                            IsPermission = p.IsPermission,
                            CategoryServiceName = p.CategoryService.CategoryServiceName
                        })
                        .ToListAsync();

                    return lockedServicePosts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách ServicePost bị khóa: {ex.Message}");
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
        public static async Task<List<ServicePostDTO>> SearchServicePostsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetServicePostsAsync();
            }

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    bool isNumeric = decimal.TryParse(searchTerm, out decimal numericValue);

                    var servicePosts = await context.ServicePosts
                        .AsNoTracking()
                        .Where(p => p.Title.ToLower().Contains(searchTerm.ToLower().Trim())
                                    || (isNumeric && p.Price > numericValue)
                                    || p.Location.ToLower().Contains(searchTerm.ToLower().Trim())
                                    || p.PhoneNumber.Contains(searchTerm.Trim()))
                        .Select(p => new ServicePostDTO
                        {
                            ServicePostId = p.ServicePostId,
                            Title = p.Title,
                            PhoneNumber = p.PhoneNumber,
                            Price = p.Price,
                            Location = p.Location,
                            Description = p.Description,
                            Image = p.Image,
                            Name = p.User.Name,
                            UserId = p.UserId,
                            IsPermission = p.IsPermission,
                            CategoryServiceId = p.CategoryServiceId,
                            CategoryServiceName = p.CategoryService.CategoryServiceName
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
        public static async Task LockServicePostAsync(int servicepostId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var servicepost = await context.ServicePosts.FirstOrDefaultAsync(u => u.ServicePostId == servicepostId);
                    if (servicepost == null)
                    {
                        throw new KeyNotFoundException($"ServicePost với ID {servicepostId} không tồn tại.");
                    }

                    servicepost.IsPermission = 0;
                    context.ServicePosts.Update(servicepost);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi khóa ServicePost: {ex.Message}");
            }
        }
        public static async Task UnLockServicePostAsync(int servicepostId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var servicepost = await context.ServicePosts.FirstOrDefaultAsync(u => u.ServicePostId == servicepostId);
                    if (servicepost == null)
                    {
                        throw new KeyNotFoundException($"ServicePost với ID {servicepostId} không tồn tại.");
                    }

                    servicepost.IsPermission = 1;
                    context.ServicePosts.Update(servicepost);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi mở khóa ServicePost: {ex.Message}");
            }
        }
        public static async Task<List<ServicePostDTO>> GetServicePostsByUserIdAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var servicePosts = await context.ServicePosts
                        .AsNoTracking()
                        .Where(p => p.UserId == userId)
                        .Select(p => new ServicePostDTO
                        {
                            ServicePostId = p.ServicePostId,
                            Title = p.Title,
                            PhoneNumber = p.PhoneNumber,
                            Price = p.Price,
                            Location = p.Location,
                            Description = p.Description,
                            Name = p.User.Name,
                            Image = p.Image,
                            UserId = p.UserId,
                            IsPermission = p.IsPermission,
                            CategoryServiceId = p.CategoryServiceId,
                            CategoryServiceName = p.CategoryService.CategoryServiceName
                        })
                        .ToListAsync();

                    return servicePosts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách dịch vụ của userId {userId}: {ex.Message}");
            }
        }

    }
}
