using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Enums;

namespace DataAccess
{
    public class TransactionDAO
    {
        private readonly ApplicationDbContext _context;

        public TransactionDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Transaction> AddTransaction(decimal amount, string description, int userId)
        {
            
            var transaction = new Transaction
            {
                Amount = amount,
                Description = description,
                UserId = userId,
                Status = TransactionStatus.Pending, // Set default status
                CreatedAt = DateTime.UtcNow // Use current UTC time
            };
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        
        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            var existingTransaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Description != null && transaction.Description.ToLower().Contains(t.Description.ToLower())  && t.Amount == transaction.Amount);
            if (existingTransaction == null)
            {
                throw new KeyNotFoundException($"Transaction with Description '{transaction.Description}' and Amount {transaction.Amount} not found.");
            }
            existingTransaction.CusumBalance = transaction.CusumBalance;
            existingTransaction.When = transaction.When;
            existingTransaction.BankSubAccID = transaction.BankSubAccID;
            existingTransaction.SubAccID = transaction.SubAccID;
            existingTransaction.BankName = transaction.BankName;
            existingTransaction.bankAbbreviation = transaction.bankAbbreviation;
            existingTransaction.CorresponsiveName = transaction.CorresponsiveName;
            existingTransaction.CorresponsiveAccount = transaction.CorresponsiveAccount;
            existingTransaction.CorresponsiveBankId = transaction.CorresponsiveBankId;
            existingTransaction.CorresponsiveBankName = transaction.CorresponsiveBankName;
            existingTransaction.Status = transaction.Status;
            existingTransaction.CassoId = transaction.CassoId;
            existingTransaction.TId = transaction.TId;
            _context.Transactions.Update(existingTransaction);
            await _context.SaveChangesAsync();
            return existingTransaction;
        }
        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _context.Transactions.Include(t => t.User).ToListAsync();
        }
        public async Task<Transaction?> GetTransactionById(int id)
        {
            return await _context.Transactions.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<List<Transaction>> GetTransactionsByUserId(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.User) // Include the associated User for each transaction
                .ToListAsync();
        }

        public async Task<bool> DoesTransactionProcessedAsync(int cassoId)
        {
            // Check if a transaction with the given CassoID exists
            var exists = await _context.Transactions
                .AnyAsync(t => t.CassoId == cassoId);
            return exists;
        }

    }
}
