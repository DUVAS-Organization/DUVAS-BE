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
    public class RentalServiceListRepository : IRentalServiceListRepository
    {
        public async Task DeleteRentalServiceListAsync(RentalServiceList b) => await RentalServiceListDAO.DeleteRentalServiceListAsync(b);
        public async Task<RentalServiceList> GetRentalServiceListByIdAsync(int id) => await RentalServiceListDAO.FindRentalServiceListByIdAsync(id);
        public async Task<List<RentalServiceListDTO>> GetRentalServiceListsAsync() => await RentalServiceListDAO.GetRentalServiceListsAsync();
        public async Task SaveRentalServiceListAsync(RentalServiceList b) => await RentalServiceListDAO.SaveRentalServiceListAsync(b);
        public async Task UpdateRentalServiceListAsync(RentalServiceList b) => await RentalServiceListDAO.UpdateRentalServiceListAsync(b);
       
    }
}
