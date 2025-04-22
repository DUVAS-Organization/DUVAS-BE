using DataAccess;
using DTO;
using DUVAS;
using Repositories.IRepository;

namespace Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public async Task<Transaction> AddTransaction(decimal amount, string description, int userId) => await new TransactionDAO(new ApplicationDbContext()).AddTransaction(amount, description, userId);
        public async Task<Transaction> UpdateTransaction(Transaction transaction) => await new TransactionDAO(new ApplicationDbContext()).UpdateTransaction(transaction);
        public async Task<List<Transaction>> GetAllTransactions() => await new TransactionDAO(new ApplicationDbContext()).GetAllTransactions();
        public async Task<Transaction?> GetTransactionById(int id) => await new TransactionDAO(new ApplicationDbContext()).GetTransactionById(id);
        public async Task<List<Transaction>> GetTransactionsByUserId(int userId) => await new TransactionDAO(new ApplicationDbContext()).GetTransactionsByUserId(userId);
        public Task<bool> DoesTransactionProcessedAsync(int cassoId) => new TransactionDAO(new ApplicationDbContext()).DoesTransactionProcessedAsync(cassoId);
        public Task<bool> IsTransactionPaidAsync(string description) => new TransactionDAO(new ApplicationDbContext()).IsTransactionPaidAsync(description);
        public async Task<List<TransactionAdminDTO>> GetAllTransactionAdminView()
            => await new TransactionDAO(new ApplicationDbContext()).GetAllTransactionAdminView();
        public async Task<List<TransactionAdminDTO>> GetAllDeposits()
            => await new TransactionDAO(new ApplicationDbContext()).GetAllDeposits();

        public async Task<List<TransactionAdminDTO>> GetAllWithdrawals()
            => await new TransactionDAO(new ApplicationDbContext()).GetAllWithdrawals();

        public async Task<decimal> GetTotalDeposits()
            => await new TransactionDAO(new ApplicationDbContext()).GetTotalDeposits();

        public async Task<decimal> GetTotalWithdrawals()
            => await new TransactionDAO(new ApplicationDbContext()).GetTotalWithdrawals();
        public async Task<decimal> GetTotalRevenue()
            => await new TransactionDAO(new ApplicationDbContext()).GetTotalRevenue();

        public async Task<Dictionary<string, decimal>> GetMonthlyRevenue()
            => await new TransactionDAO(new ApplicationDbContext()).GetMonthlyRevenue();

    }
}
