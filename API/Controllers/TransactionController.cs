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
        [Authorize]
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
    }
}