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
    }
}
