using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO
    {
        private readonly ApplicationDbContext _context;

        public UserDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<UserDTO>> GetUsersAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var users = await context.Users
                        .AsNoTracking()
                        .Select(p => new UserDTO
                        {
                            UserId = p.UserId,
                            UserName = p.UserName,
                            Name = p.Name,
                            Gmail = p.Gmail,
                            Phone = p.Phone,
                            Address = p.Address,
                            Sex = p.Sex,
                            ProfilePicture = p.ProfilePicture,
                            Money = p.Money,
                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return users;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<User> FindUserByIdAsync(int userId)
        {
            User user = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    user = await context.Users.SingleOrDefaultAsync(x => x.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public static async Task SaveUserAsync(User user)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateUserAsync(User user)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(user).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteUserAsync(User user)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingUser = await context.Users.SingleOrDefaultAsync(c => c.UserId == user.UserId);
                    if (existingUser != null)
                    {
                        context.Users.Remove(existingUser);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<UserDTO>> SearchUsersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetUsersAsync();
            }

            try
            {
                using (var context = new ApplicationDbContext())
                {

                    bool isNumeric = int.TryParse(searchTerm, out int numericValue);

                    var user = await context.Users
                        .AsNoTracking()
                        .Where(p => p.UserName.ToLower().Contains(searchTerm.ToLower().Trim())
                                //|| (isNumeric && p.Price > numericValue)
                                )
                        .Select(p => new UserDTO
                        {
                            UserId = p.UserId,
                            UserName = p.UserName,
                            Name = p.Name,
                            Gmail = p.Gmail,
                            Phone = p.Phone,
                            Address = p.Address,
                            Sex = p.Sex,
                            ProfilePicture = p.ProfilePicture,
                            Money = p.Money,
                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,
                            //Price = p.Price,
                        })
                        .ToListAsync();

                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public static async Task<bool> UpdatePasswordAsync(string emailOrPhone, string password)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await context.Users
                        .FirstOrDefaultAsync(u => u.Gmail == emailOrPhone || u.Phone == emailOrPhone);

                    if (user == null)
                    {
                        return false;
                    }
                    user.Password = password;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating password: {ex.Message}");
                return false;
            }
        }
        
        public static async Task<User?> FindUserByEmailOrPhoneAsync(string emailOrPhone)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await context.Users
                        .AsNoTracking()
                        .SingleOrDefaultAsync(u => 
                            (u.Gmail != null && u.Gmail.ToLower() == emailOrPhone.ToLower()) || 
                            (u.Phone != null && u.Phone == emailOrPhone));

                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static async Task<User?> FindUserByUsername(string username)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await context.Users
                        .AsNoTracking()
                        .SingleOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());

                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding user by username: {ex.Message}");
                return null;
            }
        }
    }
}
