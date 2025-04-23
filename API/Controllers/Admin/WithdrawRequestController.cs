using BusinessObject.Enums;
using DTO;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.IdentityModel.Tokens;
using Repositories.IRepository;

namespace API.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
public class WithdrawRequestController : ODataController
{
    private readonly IWithdrawRequestRepository _withdrawRequestRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public WithdrawRequestController(IWithdrawRequestRepository withdrawRequestRepository, ITransactionRepository transactionRepository, IConfiguration configuration, IUserRepository userRepository)
    {
        _withdrawRequestRepository = withdrawRequestRepository;
        _transactionRepository = transactionRepository;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    [HttpGet("")]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<WithdrawRequest>>> GetWithdrawRequestsList(string searchTerm = null)
    {

        if (searchTerm.IsNullOrEmpty())
        {
            return Ok(await _withdrawRequestRepository.GetAllAsync());
        }
        var withDrawReq = await _withdrawRequestRepository.SearchWithdrawRequestsAsync(searchTerm);

        return Ok(withDrawReq);
    }


    [HttpPatch("{id}/status")]
    [Authorize("Admin")]
    public async Task<IActionResult> RejectWithdrawRequestStatus(int id, RejectTransactionDTO rejectTransactionDTO)
    {
        var withdrawRequest = await _withdrawRequestRepository.GetByIdAsync(id);
        if (withdrawRequest == null)
        {
            return NotFound("Withdraw request not found.");
        }

        if (withdrawRequest.Status != WithdrawRequestStatus.Pending)
        {
            return BadRequest("Withdraw request is not pending.");
        }
        withdrawRequest.Status = WithdrawRequestStatus.Rejected;
        withdrawRequest.Reason = rejectTransactionDTO.reason;
        withdrawRequest.UpdatedAt = DateTime.UtcNow;

        await _withdrawRequestRepository.UpdateStatusAsync(withdrawRequest);
        await _userRepository.UpdateUserMoneyAsync(withdrawRequest.UserId, -withdrawRequest.Amount);
        return Ok(new { message = "Withdraw request status updated successfully." });
    }

}