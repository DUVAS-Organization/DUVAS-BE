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
    }
}
