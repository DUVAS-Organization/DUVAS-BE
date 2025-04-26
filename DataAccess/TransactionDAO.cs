using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Enums;
using BusinessObject;

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
                Status = TransactionStatus.Pending, 
                CreatedAt = DateTime.Now 
            };
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            var existingTransaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Description != null && transaction.Description.ToLower().Contains(t.Description.ToLower()) && t.Amount == transaction.Amount);
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
                .OrderByDescending(d => d.CreatedAt) // Order Descending By CreatedAt
                .ToListAsync();
        }

        public async Task<bool> DoesTransactionProcessedAsync(int cassoId)
        {
            // Check if a transaction with the given CassoID exists
            var exists = await _context.Transactions
                .AnyAsync(t => t.CassoId == cassoId);
            return exists;
        }
        public async Task<bool> IsTransactionPaidAsync(string description)
        {
            var transaction = await _context.Transactions
                  .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Description != null && t.Description.ToLower() == description.ToLower());
            if (transaction != null && transaction.Status == TransactionStatus.Paid)
            {
                // ✅ Chỉ tạo thông báo nếu là giao dịch nạp tiền
                if (transaction.Amount > 0)
                {
                    var notification = new Notification
                    {
                        UserId = transaction.UserId,
                        Type = "TransactionPaid",
                        Message = $"Giao dịch của bạn với mô tả \"{transaction.Description}\" đã được xác nhận thành công.",
                        RedirectUrl = "/transactions",
                        CreatedDate = DateTime.Now,
                        IsRead = false
                    };
                    await NotificationDAO.CreateNotificationAsync(notification);
                }

                return true;
            }

            return false;
            //return transaction != null && transaction.Status == TransactionStatus.Paid;
        }
        public async Task<List<TransactionAdminDTO>> GetAllTransactionAdminView()
        {
            return await _context.Transactions
                .Include(t => t.User)
                .Where(t => t.When != null)
                .Select(t => new TransactionAdminDTO
                {
                    UserName = t.User != null ? t.User.Name : "Unknown",
                    Gmail = t.User != null ? t.User.Gmail : "Unknown",
                    Amount = t.Amount,
                    Description = t.Description,
                    When = t.When,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                }).ToListAsync();
        }
        public async Task<List<TransactionAdminDTO>> GetAllDeposits()
        {
            return await _context.Transactions
                .Include(t => t.User)
                .Where(t => t.Amount > 0 && t.When != null)
                .Select(t => new TransactionAdminDTO
                {
                    UserName = t.User != null ? t.User.Name : "Unknown",
                    Gmail = t.User != null ? t.User.Gmail : "Unknown",
                    Amount = t.Amount,
                    Description = t.Description,
                    When = t.When,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                }).ToListAsync();
        }

        public async Task<List<TransactionAdminDTO>> GetAllWithdrawals()
        {
            return await _context.Transactions
                .Include(t => t.User)
                .Where(t => t.Amount < 0 && t.When != null)
                .Select(t => new TransactionAdminDTO
                {
                    UserName = t.User != null ? t.User.Name : "Unknown",
                    Gmail = t.User != null ? t.User.Gmail : "Unknown",
                    Amount = t.Amount,
                    Description = t.Description,
                    When = t.When,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                }).ToListAsync();
        }

        public async Task<decimal> GetTotalDeposits()
        {
            return await _context.Transactions
                .Where(t => t.Amount > 0 && t.When != null)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalWithdrawals()
        {
            return await _context.Transactions
                .Where(t => t.Amount > 0 && t.When != null)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalRevenue()
        {
            var deposit = await GetTotalDeposits();
            var withdrawal = await GetTotalWithdrawals();
            return deposit + withdrawal;
        }

        public async Task<Dictionary<string, decimal>> GetMonthlyRevenue()
        {
            var transactions = await _context.Transactions
                .Where(t => t.When != null)
                .ToListAsync();

            var monthlyRevenue = transactions
                .GroupBy(t => t.When.Value.ToString("yyyy-MM"))
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key, // yyyy-MM
                    g => g.Sum(t => t.Amount)
                );

            return monthlyRevenue;
        }
    }
}