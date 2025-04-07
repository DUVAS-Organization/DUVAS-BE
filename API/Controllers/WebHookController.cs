using System.Text;
using BusinessObject.Enums;
using DTO.WebHook;
using DUVAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.IRepository;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WebHookController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWithdrawRequestRepository _withdrawRequestRepository;
    public WebHookController(IConfiguration configuration, ITransactionRepository transactionRepository, IUserRepository userRepository, IWithdrawRequestRepository withdrawRequestRepository)
    {
        _configuration = configuration;
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _withdrawRequestRepository = withdrawRequestRepository;
    }
    [HttpPost("")]
    [AllowAnonymous]
    public async Task<IActionResult> CassoWebhook()
    {
        string requestBody = await ReadRequestBodyAsync();
        JObject webHookRequest = JsonConvert.DeserializeObject<JObject>(requestBody) ?? throw new InvalidOperationException("Error deserializing JSON response: Deserialized object is null.");

        if (webHookRequest != null)
        {
            Console.WriteLine(webHookRequest);
        }
        else
        {
            Console.WriteLine("No 'data' array found in the request.");
        }

        return Ok();
    }

    [HttpPost("casso")]
    [AllowAnonymous]
    public async Task<IActionResult> PayOsWebhook()
    {
        if (Request.Headers.TryGetValue("Secure-Token", out StringValues secureToken))
        {
            if (secureToken.Equals(_configuration["CassoSettings:SecretKey"]))
            {
                string requestBody = await ReadRequestBodyAsync();
                WebHookRequest webHookRequest = JsonConvert.DeserializeObject<WebHookRequest>(requestBody) ?? throw new InvalidOperationException("Error deserializing JSON response: Deserialized object is null.");
                if (_transactionRepository.DoesTransactionProcessedAsync(Convert.ToInt32(webHookRequest.data.FirstOrDefault().Id)).Result)
                {
                    return Ok(new { success = true });
                }
                Transaction transaction = new Transaction
                {
                    CassoId = Convert.ToInt32(webHookRequest.data.FirstOrDefault().Id),
                    TId = webHookRequest.data.FirstOrDefault().Tid,
                    Description = webHookRequest.data.FirstOrDefault().Description,
                    Amount = Convert.ToDecimal(webHookRequest.data.FirstOrDefault().Amount),
                    CusumBalance = Convert.ToDecimal(webHookRequest.data.FirstOrDefault().Cusum_balance),
                    When = DateTime.Parse(webHookRequest.data.FirstOrDefault().When),
                    BankSubAccID = webHookRequest.data.FirstOrDefault().Bank_sub_acc_id,
                    SubAccID = webHookRequest.data.FirstOrDefault().SubAccId,
                    BankName = webHookRequest.data.FirstOrDefault().BankName,
                    bankAbbreviation = webHookRequest.data.FirstOrDefault().BankAbbreviation,
                    CorresponsiveName = webHookRequest.data.FirstOrDefault().CorresponsiveName,
                    CorresponsiveAccount = webHookRequest.data.FirstOrDefault().CorresponsiveAccount,
                    CorresponsiveBankId = webHookRequest.data.FirstOrDefault().CorresponsiveBankId,
                    CorresponsiveBankName = webHookRequest.data.FirstOrDefault().CorresponsiveBankName,
                    Status = TransactionStatus.Paid
                };
                transaction = await _transactionRepository.UpdateTransaction(transaction);
                Console.WriteLine(webHookRequest.data.FirstOrDefault().Amount);
                Console.WriteLine(Convert.ToInt32(webHookRequest.data.FirstOrDefault().Amount));
                if (Convert.ToInt32(webHookRequest.data.FirstOrDefault().Amount) < 0)
                {
                    await _withdrawRequestRepository.WebHookConfirm(transaction.Id);
                }
                else
                {
                    await _userRepository.UpdateUserMoneyAsync(transaction.UserId, transaction.Amount);
                }
                return Ok(new { success = true });
            }
            return BadRequest($"Invalid Token: {secureToken}");
        }
        return BadRequest("Secure-Token header not found");
    }

    private async Task<string> ReadRequestBodyAsync()
    {
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            return await reader.ReadToEndAsync();
        }
    }
}