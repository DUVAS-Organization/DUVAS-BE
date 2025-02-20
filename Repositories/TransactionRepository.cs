using DataAccess;
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

    }
}
