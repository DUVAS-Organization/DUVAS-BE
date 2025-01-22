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
    public class ContractRepository : IContractRepository
    {
        public async Task DeleteContractAsync(Contract b) => await ContractDAO.DeleteContractAsync(b);
        public async Task<Contract> GetContractByIdAsync(int id) => await ContractDAO.FindContractByIdAsync(id);
        public async Task<List<ContractDTO>> GetContractsAsync() => await ContractDAO.GetContractsAsync();
        public async Task SaveContractAsync(Contract b) => await ContractDAO.SaveContractAsync(b);
        public async Task UpdateContractAsync(Contract b) => await ContractDAO.UpdateContractAsync(b);
       
    }
}
