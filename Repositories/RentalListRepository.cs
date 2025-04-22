using DataAccess;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class RentalListRepository : IRentalListRepository
    {
        private readonly ApplicationDbContext _context;

        public RentalListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteRentalListAsync(RentalList b) => await RentalListDAO.DeleteRentalListAsync(b);
        public async Task<RentalList> GetRentalListByIdAsync(int id) => await RentalListDAO.FindRentalListByIdAsync(id);
        public async Task<List<RentalListDTO>> GetRentalListsAsync() => await RentalListDAO.GetRentalListsAsync();
        public async Task<List<RentalListDTO>> GetRentalsByUserIdAsync(int id) => await RentalListDAO.GetRentalsByUserIdAsync(id);
        public async Task SaveRentalListAsync(RentalList b) => await RentalListDAO.SaveRentalListAsync(b);
        public async Task UpdateRentalListAsync(RentalList b) => await RentalListDAO.UpdateRentalListAsync(b);
        public async Task UpdateRentalListContractAsync(int rentalId, int contractId) => await RentalListDAO.UpdateRentalListContractAsync(rentalId, contractId);
        public async Task<RentalList> GetRentalListByRoomIdAsync(int roomId) => await RentalListDAO.GetRentalListByRoomIdAsync(roomId);
        public async Task UpdateRentalListStatusAsync(int rentalId, int status) => await RentalListDAO.UpdateRentalListStatusAsync(rentalId, status);
        public async Task<RentalList> GetRentalListByRoomIdAndRenterIdAsync(int roomId, int renterId) => await RentalListDAO.GetRentalListByRoomIdAndRenterIdAsync(roomId, renterId);
        public async Task<List<RentalList>> GetPendingRentalListsByRoomIdAsync(int roomId) => await RentalListDAO.GetPendingRentalListsByRoomIdAsync(roomId);
        public async Task<List<RentalList>> GetConfirmedRentalListsByRoomIdAsync(int roomId) => await RentalListDAO.GetConfirmedRentalListsByRoomIdAsync(roomId);
    }
}