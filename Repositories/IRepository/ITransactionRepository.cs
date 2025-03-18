using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ITransactionRepository
    {

        Task<Transaction> AddTransaction(decimal amount, string description, int userId);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction?> GetTransactionById(int id);
        Task<List<Transaction>> GetTransactionsByUserId(int userId);
        Task<bool> DoesTransactionProcessedAsync(int cassoId);
        Task<bool> IsTransactionPaidAsync(string description);
    }
}