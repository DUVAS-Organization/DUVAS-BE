using DUVAS;

namespace Repositories.IRepository;

public interface IWithdrawRequestRepository
{
    Task AddAsync(int userId, decimal amount, int TransactionId, string BankCode, string AccountNumber);
    Task UpdateAsync(WithdrawRequest withdrawRequest);
    Task<List<WithdrawRequest>> GetAllAsync();
    Task<List<WithdrawRequest>> GetAllByUserIdAsync(int userId);
    Task<WithdrawRequest?> GetByIdAsync(int withdrawRequestId);
    Task WebHookConfirm(int transactionId);
}