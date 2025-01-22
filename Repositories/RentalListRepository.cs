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
    public class RentalListRepository : IRentalListRepository
    {
        public async Task DeleteRentalListAsync(RentalList b) => await RentalListDAO.DeleteRentalListAsync(b);
        public async Task<RentalList> GetRentalListByIdAsync(int id) => await RentalListDAO.FindRentalListByIdAsync(id);
        public async Task<List<RentalListDTO>> GetRentalListsAsync() => await RentalListDAO.GetRentalListsAsync();
        public async Task SaveRentalListAsync(RentalList b) => await RentalListDAO.SaveRentalListAsync(b);
        public async Task UpdateRentalListAsync(RentalList b) => await RentalListDAO.UpdateRentalListAsync(b);
       
    }
}
