using DataAccess;
using DUVAS;
using Repositories.IRepository;

namespace Repositories;

public class WithdrawRequestRepository : IWithdrawRequestRepository
{
    public Task AddAsync(int userId, decimal amount, int transactionId, string BankCode, string AccountNumber) => new WithdrawRequestDAO(new ApplicationDbContext()).AddAsync(userId, amount, transactionId, BankCode, AccountNumber);

    public Task UpdateAsync(WithdrawRequest withdrawRequest) => new WithdrawRequestDAO(new ApplicationDbContext()).UpdateAsync(withdrawRequest);

    public Task<List<WithdrawRequest>> GetAllAsync() => new WithdrawRequestDAO(new ApplicationDbContext()).GetAllAsync();

    public Task<List<WithdrawRequest>> GetAllByUserIdAsync(int userId) => new WithdrawRequestDAO(new ApplicationDbContext()).GetAllByUserIdAsync(userId);
    public Task<WithdrawRequest?> GetByIdAsync(int withdrawRequestId) => new WithdrawRequestDAO(new ApplicationDbContext()).GetByIdAsync(withdrawRequestId);
    public Task WebHookConfirm(int transactionId) => new WithdrawRequestDAO(new ApplicationDbContext()).WebHookConfirm(transactionId);
}