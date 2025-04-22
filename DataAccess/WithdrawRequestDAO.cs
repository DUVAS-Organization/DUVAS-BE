using BusinessObject;
using BusinessObject.Enums;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class WithdrawRequestDAO
{
    private readonly ApplicationDbContext _context;

    public WithdrawRequestDAO(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add a new WithdrawRequest
    public async Task AddAsync(int userId, decimal amount, int transactionId, string BankCode, string AccountNumber)
    {
        var withdrawRequest = new WithdrawRequest
        {
            UserId = userId,
            Amount = amount, // Assuming you want to include Amount in the WithdrawRequest
            BankCode = BankCode,
            AccountNumber = AccountNumber,
            Status = WithdrawRequestStatus.Pending, // Set default status to Pending
            Reason = null, // Default to null for Reason
            CreatedAt = DateTime.UtcNow, // Current UTC time for CreatedAt
            UpdatedAt = DateTime.UtcNow, // Current UTC time for UpdatedAt
            TransactionId = transactionId
        };
        await _context.WithdrawRequests.AddAsync(withdrawRequest);
        await _context.SaveChangesAsync();

        // ✅ Thêm thông báo
        var notification = new Notification
        {
            UserId = userId,
            Type = "WithdrawRequest",
            Message = $"Bạn đã gửi yêu cầu rút {Math.Abs(amount):N0}đ về tài khoản {AccountNumber}.",
            RedirectUrl = "/withdraw-requests", // Có thể thay đổi thành link chi tiết nếu cần
            CreatedDate = DateTime.UtcNow,
            IsRead = false
        };

        await NotificationDAO.CreateNotificationAsync(notification);
    
    }

    // Update an existing WithdrawRequest
    public async Task UpdateAsync(WithdrawRequest withdrawRequest)
    {
        var existingRequest = await _context.WithdrawRequests.FindAsync(withdrawRequest.Id);
        if (existingRequest == null)
        {
            throw new KeyNotFoundException($"WithdrawRequest with ID {withdrawRequest.Id} not found.");
        }

        // Update fields
        existingRequest.Status = withdrawRequest.Status;
        existingRequest.Reason = withdrawRequest.Reason;
        existingRequest.UpdatedAt = DateTime.UtcNow;

        _context.WithdrawRequests.Update(existingRequest);
        await _context.SaveChangesAsync();
    }

    // Get all WithdrawRequests
    public async Task<List<WithdrawRequest>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        return await _context.WithdrawRequests
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    // Get all WithdrawRequests by UserId
    public async Task<List<WithdrawRequest>> GetAllByUserIdAsync(int userId, int page = 1, int pageSize = 10)
    {
        return await _context.WithdrawRequests
            .Where(w => w.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<WithdrawRequest?> GetByIdAsync(int withdrawRequestId)
    {
        var withdrawRequest = await _context.WithdrawRequests
            .Where(w => w.Id == withdrawRequestId)
            .Include(w => w.User)
            .Include(w => w.Transaction)
            .FirstOrDefaultAsync();
        return withdrawRequest;
    }

    public async Task WebHookConfirm(int transactionId)
    {
        var existingRequest = await _context.WithdrawRequests
            .Include(w => w.User) // cần để lấy tên người dùng hoặc dùng UserId
            .FirstOrDefaultAsync(w => w.TransactionId == transactionId);
        if (existingRequest == null)
        {
            throw new KeyNotFoundException($"WithdrawRequest with Transaction ID {transactionId} not found.");
        }

        existingRequest.Status = WithdrawRequestStatus.Approved;
        existingRequest.UpdatedAt = DateTime.UtcNow;
        _context.WithdrawRequests.Update(existingRequest);

        // ✅ Tạo thông báo mới
        var notification = new Notification
        {
            UserId = existingRequest.UserId,
            Type = "WithdrawPaid",
            Message = $"Yêu cầu rút {existingRequest.Amount:N0}đ của bạn đã được phê duyệt.",
            RedirectUrl = "/transactions",
            CreatedDate = DateTime.Now,
            IsRead = false
        };
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<List<WithdrawRequest>> SearchWithdrawRequestsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }

        try
        {
            var withdrawRequest = await _context.WithdrawRequests
                .AsNoTracking()
                .Where(p => p.AccountNumber.Contains(searchTerm.Trim()))
                .ToListAsync();

            return withdrawRequest;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}