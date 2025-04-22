using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IUserRepository
    {
        Task SaveUserAsync(User b);
        Task<User> GetUserByIdAsync(int id);
        Task<User?> GetUserByGmailOrPhoneAsync(string gmailOrPhone);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> UpdatePasswordAsync(string username, string password);
        Task DeleteUserAsync(User b);
        Task UpdateUserAsync(User b);
        Task<List<UserDTO>> GetUsersAsync();
        Task<List<UserDTO>> SearchUsersAsync(string searchTerm);
        Task UpdateUserMoneyAsync(int userId, decimal amount);
        
        Task<List<BankAccounts>> GetUserBankAccounts(int userId);
        Task<BankAccounts> CreateNewBankAccounts(int userId, BankAccountsDTO bankAccount);
        Task<bool> CheckBankAccountExistsAsync(string accountNumber, string bankCode);
        Task<Boolean> UpdateBankAccountStatus(int userId, int BankAccountId, bool active);
        Task<BankAccounts> GetUserBankAccountByIdAndUserIdAsync(int userId, int bankAccountId);
        Task<decimal> GetUserMoneyWithIdAsync(int userId);
        Task<bool> CheckUserBalanceAsync(int userId, decimal amount);
        Task LockUserAsync(int userId);
        Task UnLockUserAsync(int userId);
        Task<List<UserDTO>> GetListUserLockAsync();
        Task<List<UserDTO>> GetListUserActiveAsync();
        Task<List<UserDTO>> GetListUpRoleLandLord();
        Task<List<UserDTO>> GetListUpRoleService();
        Task AcceptUpRoleLandLordAsync(int userId);
        Task AcceptUpRoleServiceAsync(int userId);
        Task CancelUpRoleLandLordAsync(int userId);
        Task CancelUpRoleServiceAsync(int userId);
        Task<List<LandlordLicenseDTO>> GetLandlordLicensesByUserIdAsync(int userId);
        Task<List<ServiceLicenseDTO>> GetServiceLicensesByUserIdAsync(int userId);


    }
}
