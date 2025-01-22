using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IRentalListRepository
    {
        Task SaveRentalListAsync(RentalList b);
        Task<RentalList> GetRentalListByIdAsync(int id);
        Task DeleteRentalListAsync(RentalList b);
        Task UpdateRentalListAsync(RentalList b);
        Task<List<RentalListDTO>> GetRentalListsAsync();
    }
}
