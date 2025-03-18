using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;

namespace API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class WithDrawController : ControllerBase
{
    private readonly IWithdrawRequestRepository _withdrawRequestRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    public WithDrawController(IWithdrawRequestRepository withdrawRequestRepository, IConfiguration configuration, ITransactionRepository transactionRepository, IUserRepository userRepository)
    {
        _withdrawRequestRepository = withdrawRequestRepository;
        _configuration = configuration;
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
    }
    [HttpPost("")]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> CreateWithDrawRequest([FromBody] WithdrawRequestDTO withdrawRequestDto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
        {
            return BadRequest("UserId claim not found.");
        }
        int.TryParse(userIdClaim.Value, out int userId);
        decimal money = _userRepository.GetUserMoneyWithIdAsync(userId).Result;
        if (money < withdrawRequestDto.Amount)
        {
            return BadRequest("Your balance is not enough.");
        }
        Guid newUuid = Guid.NewGuid();
        string uuid = newUuid.ToString();
        uuid = uuid.Replace("-", "");
        var transaction = await _transactionRepository.AddTransaction(-withdrawRequestDto.Amount, uuid, userId);
        var bankAccount = _userRepository.GetUserBankAccountByIdAndUserIdAsync(userId, withdrawRequestDto.BankAccountId).Result;
        if (bankAccount == null)
        {
            return StatusCode(500, "Server error.");
        }
        await _withdrawRequestRepository.AddAsync(userId, -withdrawRequestDto.Amount, transaction.Id, bankAccount.BankCode, bankAccount.AccountNumber);
        return Ok(new { message = "Withdraw request create successful." });
    }
    [HttpGet("payment-qr")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetWithdrawPaymentLink([FromQuery] int id)
    {
        var withDrawReq = await _withdrawRequestRepository.GetByIdAsync(id);
        if (withDrawReq == null)
        {
            return BadRequest();
        }

        if (withDrawReq.Transaction == null)
        {
            return BadRequest();
        }
        var qrCodeImage = "https://img.vietqr.io/image/" + withDrawReq.BankCode + "-" + withDrawReq.AccountNumber + "-print.jpg?amount=" +
                          (-withDrawReq.Transaction.Amount) + "&addInfo=" + withDrawReq.Transaction.Description + "&accountName=" + _configuration["CassoSettings:AccountName"];
        return Ok(new { QRCode = qrCodeImage });
    }

    [HttpGet("user")]
    [Authorize(Policy = "User")]
    public async Task<ActionResult<IEnumerable<WithdrawRequest>>> GetUserWithdrawRequests()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
        {
            return BadRequest("UserId claim not found.");
        }
        int.TryParse(userIdClaim.Value, out int userId);
        var withDrawReq = await _withdrawRequestRepository.GetAllByUserIdAsync(userId);
        return Ok(withDrawReq);
    }
}