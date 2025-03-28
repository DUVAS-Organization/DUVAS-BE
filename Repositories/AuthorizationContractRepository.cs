using BusinessObject;
using DataAccess;
using DTO;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class AuthorizationContractRepository : IAuthorizationContractRepository
    {
        public async Task DeleteAuthorizationContractAsync(AuthorizationContract contract)
            => await AuthorizationContractDAO.DeleteAuthorizationContractAsync(contract);

        public async Task<AuthorizationContract> GetAuthorizationContractByIdAsync(int id)
            => await AuthorizationContractDAO.FindAuthorizationContractByIdAsync(id);

        public async Task<List<AuthorizationContractDTO>> GetAuthorizationContractsAsync()
            => await AuthorizationContractDAO.GetAuthorizationContractsAsync();

        public async Task<List<AuthorizationContractDTO>> GetAuthorizationContractsByUserAsync(int createdById)
            => await AuthorizationContractDAO.GetAuthorizationContractsByUserAsync(createdById);

        public async Task SaveAuthorizationContractAsync(AuthorizationContract contract)
            => await AuthorizationContractDAO.SaveAuthorizationContractAsync(contract);

        public async Task UpdateAuthorizationContractAsync(AuthorizationContract contract)
            => await AuthorizationContractDAO.UpdateAuthorizationContractAsync(contract);
    }
}