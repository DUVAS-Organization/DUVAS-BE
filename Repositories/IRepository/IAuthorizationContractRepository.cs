using BusinessObject;
using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IAuthorizationContractRepository
    {
        Task SaveAuthorizationContractAsync(AuthorizationContract contract);
        Task<AuthorizationContract> GetAuthorizationContractByIdAsync(int id);
        Task DeleteAuthorizationContractAsync(AuthorizationContract contract);
        Task UpdateAuthorizationContractAsync(AuthorizationContract contract);
        Task<List<AuthorizationContractDTO>> GetAuthorizationContractsAsync();
        Task<List<AuthorizationContractDTO>> GetAuthorizationContractsByUserAsync(int createdById);
        Task UpdateStatusAsync(int id, int status);
    }
}