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
using BusinessObject.Enums;

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
                            RoleAdmin = p.RoleAdmin,
                            RoleLandlord = p.RoleLandlord,
                            RoleService = p.RoleService,
                            RoleUser = p.RoleUser,
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

        public static async Task<List<UserDTO>> GetListUserLockAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var lockedUsers = await context.Users
                        .AsNoTracking()
                        .Where(u => u.RoleUser == 0)
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
                            RoleAdmin = p.RoleAdmin,
                            RoleLandlord = p.RoleLandlord,
                            RoleService = p.RoleService,
                            RoleUser = p.RoleUser,
                        })
                        .ToListAsync();

                    return lockedUsers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách User bị khóa: {ex.Message}");
            }
        }
        public static async Task<List<UserDTO>> GetListUserActiveAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var lockedUsers = await context.Users
                        .AsNoTracking()
                        .Where(u => u.RoleUser == 1)
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
                            RoleAdmin = p.RoleAdmin,
                            RoleLandlord = p.RoleLandlord,
                            RoleService = p.RoleService,
                            RoleUser = p.RoleUser,
                        })
                        .ToListAsync();

                    return lockedUsers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách User bị khóa: {ex.Message}");
            }
        }
        public static async Task<User> FindUserByIdAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await context.Users
                        .FirstOrDefaultAsync(x => x.UserId == userId);

                    if (user == null)
                    {
                        Console.WriteLine("User not found with ID: " + userId);
                    }
                    //else
                    //{
                    //    Console.WriteLine($"User found. Money: {user.Money}");
                    //}

                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.Message);
            }
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
                                || p.Gmail.ToLower().Contains(searchTerm.ToLower().Trim())
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
                            RoleAdmin = p.RoleAdmin,
                            RoleLandlord = p.RoleLandlord,
                            RoleService = p.RoleService,
                            RoleUser = p.RoleUser,
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
        public static async Task UpdateUserMoneyAsync(int userId, decimal amount)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                    if (user == null)
                    {
                        throw new KeyNotFoundException($"User với ID {userId} không tồn tại.");
                    }

                    // Cập nhật số dư của User
                    user.Money += amount;
                    context.Users.Update(user);

                    // Tạo lịch sử giao dịch trong bảng InsiderTrading
                    var transaction = new InsiderTrading
                    {
                        UserId = userId,
                        Money = amount,
                        Note = amount >= 0
                            ? $"User ID {userId} vừa + {amount} vào tài khoản."
                            : $"User ID {userId} vừa - {Math.Abs(amount)} khỏi tài khoản.",
                        Status = 0, // 0: trạng thái mặc định
                        CreatedDate = DateTime.UtcNow
                    };

                    await context.InsiderTradings.AddAsync(transaction);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật số dư: {ex.Message}");
            }
        }

        // kiểm tra tiền xem đủ không
        public static async Task<bool> CheckUserBalanceAsync(int userId, decimal amount)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = await context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Money)
                    .FirstOrDefaultAsync();
                return user >= amount;
            }
        }
        public static async Task LockUserAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                    if (user == null)
                    {
                        throw new KeyNotFoundException($"User với ID {userId} không tồn tại.");
                    }

                    user.RoleUser = 0;
                    context.Users.Update(user);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi khóa User: {ex.Message}");
            }
        }
        public static async Task UnLockUserAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                    if (user == null)
                    {
                        throw new KeyNotFoundException($"User với ID {userId} không tồn tại.");
                    }

                    user.RoleUser = 1;
                    context.Users.Update(user);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi mở khóa User: {ex.Message}");
            }
        }
        
        public async Task<List<BankAccounts>> GetUserBankAccountsByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var bankAccounts = await _context.BankAccounts
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .ToListAsync();

            if (!bankAccounts.Any())
            {
                throw new KeyNotFoundException($"User with ID {userId} does not have any bank accounts.");
            }
            
            return bankAccounts;
        }
        
        
        public async Task<BankAccounts> GetUserBankAccountByIdAndUserIdAsync(int userId, int bankAccountId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var bankAccount = await _context.BankAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Id == bankAccountId);

            if (bankAccount == null)
            {
                throw new KeyNotFoundException($"Bank account with ID {bankAccountId} not found for user ID {userId}.");
            }

            return bankAccount;
        }

        
        
        public async Task<BankAccounts> CreateNewUserBankAccount(int userId, BankAccountsDTO bankAccountDto)
        {
            var newBankAccount = new BankAccounts
            {
                AccountNumber = bankAccountDto.AccountNumber,
                AccountName = bankAccountDto.AccountName,
                BankCode = bankAccountDto.BankCode,
                Status = BankAccountStatus.Active,
                UserId = userId
            };

            _context.BankAccounts.Add(newBankAccount);
            await _context.SaveChangesAsync();

            return newBankAccount;
        }

        public async Task<bool> CheckBankAccountExistsAsync(string accountNumber, string bankCode)
        {
            try
            {
                // Kiểm tra xem có bất kỳ bản ghi nào có cùng AccountNumber và BankCode hay không
                return await _context.BankAccounts
                    .AnyAsync(b => b.AccountNumber == accountNumber && b.BankCode == bankCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking bank account existence: {ex.Message}");
                throw;
            }
        }
        
        public async Task<bool> UpdateBankAccountStatus(int userId, int bankAccountId, bool active)
        {
            try
            {
                var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(
                    b => b.UserId == userId && b.Id == bankAccountId);

                if (bankAccount == null)
                {
                    throw new KeyNotFoundException($"Bank account with ID {bankAccountId} not found for user {userId}.");
                }

                bankAccount.Status = active ? BankAccountStatus.Active : BankAccountStatus.Inactive;

                _context.BankAccounts.Update(bankAccount);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error updating bank account status: {e.Message}");
                throw;
            }
        }

        public async Task<decimal> GetUserMoneyWithIdAsync(int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var money = await context.Users
                        .Where(x => x.UserId == userId)
                        .Select(x => x.Money) // Select only the Money field
                        .SingleOrDefaultAsync();

                    return money;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching user money: " + ex.Message);
            }
        }
    }
}