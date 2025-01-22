using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IContractRepository
    {
        Task SaveContractAsync(Contract b);
        Task<Contract> GetContractByIdAsync(int id);
        Task DeleteContractAsync(Contract b);
        Task UpdateContractAsync(Contract b);
        Task<List<ContractDTO>> GetContractsAsync();
    }
}
