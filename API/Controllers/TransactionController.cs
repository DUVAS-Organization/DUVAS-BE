using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IConfiguration _configuration;
        public TransactionController(ITransactionRepository transactionRepository, IConfiguration configuration)
        {
            _transactionRepository = transactionRepository;
            _configuration = configuration;
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] DepositRequest depositRequest)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return BadRequest("UserId claim not found.");
            }
            int.TryParse(userIdClaim.Value, out int userId);
            Guid newUuid = Guid.NewGuid();
            string uuid = newUuid.ToString();
            uuid = uuid.Replace("-", "");
            if (userId == 0)
            {
                return BadRequest("Invalid UserId.");
            }
            await _transactionRepository.AddTransaction(depositRequest.Amount, uuid, userId);
            string QRCodeImage = "https://img.vietqr.io/image/" + _configuration["CassoSettings:BankId"] + "-" + _configuration["CassoSettings:AccountNo"] + "-print.jpg?amount=" + depositRequest.Amount + "&addInfo=" + uuid + "&accountName=" + _configuration["CassoSettings:AccountName"];
            return Ok(new { QRCode = QRCodeImage });
        }

        [HttpGet()]
        public async Task<IActionResult> CheckTransactionStatus(string description)
        {
            bool isPaid = await _transactionRepository.IsTransactionPaidAsync(description);
            return Ok(new { isPaid });
        }

        [HttpGet("GetTransactions")]
        [Authorize] // moi sua
        public async Task<IActionResult> GetTransactions()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return BadRequest("UserId claim not found.");
            }
            int.TryParse(userIdClaim.Value, out int userId);
            var transactions = _transactionRepository.GetTransactionsByUserId(userId);
            return Ok(new { transactions });
        }

        [HttpGet("getAll-Transaction")]
        public async Task<IActionResult> GetAllTransactionAdminView()
        {
            var transactions = await _transactionRepository.GetAllTransactionAdminView();
            return Ok(transactions);
        }
        [HttpGet("deposits")]
        public async Task<IActionResult> GetAllDeposits()
        {
            var deposits = await _transactionRepository.GetAllDeposits();
            return Ok(deposits);
        }

        [HttpGet("withdrawals")]
        public async Task<IActionResult> GetAllWithdrawals()
        {
            var withdrawals = await _transactionRepository.GetAllWithdrawals();
            return Ok(withdrawals);
        }

        [HttpGet("total-deposit")]
        public async Task<IActionResult> GetTotalDeposit()
        {
            var total = await _transactionRepository.GetTotalDeposits();
            return Ok(new { totalDeposit = total });
        }

        [HttpGet("total-withdrawal")]
        public async Task<IActionResult> GetTotalWithdrawal()
        {
            var total = await _transactionRepository.GetTotalWithdrawals();
            return Ok(new { totalWithdrawal = total });
        }

        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var totalRevenue = await _transactionRepository.GetTotalRevenue();
            return Ok(new { totalRevenue });
        }

        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> GetMonthlyRevenue()
        {
            var monthlyRevenue = await _transactionRepository.GetMonthlyRevenue();
            return Ok(monthlyRevenue);
        }
    }
}