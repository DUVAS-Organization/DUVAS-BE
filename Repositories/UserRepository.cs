using DataAccess;
using DTO;
using DUVAS;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> UpdatePasswordAsync(string username, string password) => await UserDAO.UpdatePasswordAsync(username, password);
        public async Task<User?> GetUserByGmailOrPhoneAsync(string emailOrPhone) => await UserDAO.FindUserByEmailOrPhoneAsync(emailOrPhone);
        public async Task<User?> GetUserByUsernameAsync(string username) => await UserDAO.FindUserByUsername(username);
        public async Task DeleteUserAsync(User b) => await UserDAO.DeleteUserAsync(b);
        public async Task<User> GetUserByIdAsync(int id) => await UserDAO.FindUserByIdAsync(id);
        public async Task<List<UserDTO>> GetUsersAsync() => await UserDAO.GetUsersAsync();
        public async Task SaveUserAsync(User b) => await UserDAO.SaveUserAsync(b);
        public async Task UpdateUserAsync(User b) => await UserDAO.UpdateUserAsync(b);
        public async Task<List<UserDTO>> SearchUsersAsync(string searchTerm) => await UserDAO.SearchUsersAsync(searchTerm);
        public Task<BankAccounts> CreateNewBankAccounts(int userId, BankAccountsDTO bankAccounts) =>
            new UserDAO(new ApplicationDbContext()).CreateNewUserBankAccount(userId, bankAccounts);
        public async Task<bool> CheckBankAccountExistsAsync(string accountNumber, string bankCode)
        {
            return await new UserDAO(new ApplicationDbContext()).CheckBankAccountExistsAsync(accountNumber, bankCode);
        }
        public Task<List<BankAccounts>> GetUserBankAccounts(int userId) =>
            new UserDAO(new ApplicationDbContext()).GetUserBankAccountsByIdAsync(userId);
        public Task<Boolean> UpdateBankAccountStatus(int userId, int bankAccountId, bool active) => new UserDAO(new ApplicationDbContext()).UpdateBankAccountStatus(userId, bankAccountId, active);
        public Task<BankAccounts> GetUserBankAccountByIdAndUserIdAsync(int userId, int bankAccountId) => new UserDAO(new ApplicationDbContext()).GetUserBankAccountByIdAndUserIdAsync(userId, bankAccountId);

        public Task<decimal> GetUserMoneyWithIdAsync(int userId) =>
            new UserDAO(new ApplicationDbContext()).GetUserMoneyWithIdAsync(userId);
        public async Task UpdateUserMoneyAsync(int userId, decimal amount) => await UserDAO.UpdateUserMoneyAsync(userId, amount);
        public async Task<bool> CheckUserBalanceAsync(int userId, decimal amount) => await UserDAO.CheckUserBalanceAsync(userId, amount);
        public async Task LockUserAsync(int userId) => await UserDAO.LockUserAsync(userId);
        public async Task UnLockUserAsync(int userId) => await UserDAO.UnLockUserAsync(userId);
        public async Task<List<UserDTO>> GetListUserLockAsync() => await UserDAO.GetListUserLockAsync();
        public async Task<List<UserDTO>> GetListUserActiveAsync() => await UserDAO.GetListUserActiveAsync();

        public async Task<List<UserDTO>> GetListUpRoleLandLord() => await UserDAO.GetListUpRoleLandLord();
        public async Task<List<UserDTO>> GetListUpRoleService() => await UserDAO.GetListUpRoleService();
        public async Task AcceptUpRoleLandLordAsync(int userId) => await UserDAO.AcceptUpRoleLandLordAsync(userId);
        public async Task AcceptUpRoleServiceAsync(int userId) => await UserDAO.AcceptUpRoleServiceAsync(userId);
        public async Task CancelUpRoleLandLordAsync(int userId) => await UserDAO.CancelUpRoleLandLordAsync(userId);
        public async Task CancelUpRoleServiceAsync(int userId) => await UserDAO.CancelUpRoleServiceAsync(userId);
    }
}
