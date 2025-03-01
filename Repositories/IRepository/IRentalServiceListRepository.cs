using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IRentalServiceListRepository
    {
        Task SaveRentalServiceListAsync(RentalServiceList b);
        Task<RentalServiceList> GetRentalServiceListByIdAsync(int id);
        Task DeleteRentalServiceListAsync(RentalServiceList b);
        Task UpdateRentalServiceListAsync(RentalServiceList b);
        Task<List<RentalServiceListDTO>> GetRentalServiceListsAsync();
    }
}
