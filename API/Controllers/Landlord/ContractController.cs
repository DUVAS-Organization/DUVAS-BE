using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Security.Claims;
using Utilities;
using DTO;
using System;
using API.Service;
using Repositories.IRepository;

namespace GITHUB_ACTIONS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IAuthorizationContractRepository _authorizationContractRepository;
        private readonly PdfService _pdfService;
        private readonly CloudinaryService _cloudinaryService;

        public ContractController(
            IAuthorizationContractRepository authorizationContractRepository,
            PdfService pdfService,
            CloudinaryService cloudinaryService)
        {
            _authorizationContractRepository = authorizationContractRepository;
            _pdfService = pdfService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("generate-authorization")]
        public async Task<IActionResult> GenerateAuthorizationContract([FromBody] PdfService.AuthorizationContractDetails details)
        {
            // Lấy ID của người dùng từ token JWT
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            //    return Unauthorized("User not authenticated or invalid user ID");

            // Tạo file PDF
            var pdfBytes = _pdfService.GenerateAuthorizationContractPdf(details);

            // Upload lên Cloudinary
            //var fileName = $"authorization_contract_{userId}_{DateTime.Now.Ticks}.pdf";
            var fileName = $"authorization_contract_{details.PartyAId}_{DateTime.Now.Ticks}.pdf";
            var pdfUrl = await _cloudinaryService.UploadPdfAsync(pdfBytes, fileName);



            // Lưu thông tin cơ bản vào DB
            var contract = new AuthorizationContract
            {
                ContractNumber = details.ContractNumber,
                //Date = details.Date,
                PartyAId = details.PartyAId,
                PartyBId = details.PartyBId,
                PdfUrl = pdfUrl,
                //CreatedById = userId,
                CreatedById = details.PartyAId,
                CreatedAt = DateTime.UtcNow
            };

            await _authorizationContractRepository.SaveAuthorizationContractAsync(contract);

            return Ok(new { ContractId = contract.Id, PdfUrl = pdfUrl });
        }

        [HttpGet("my-authorization-contracts")]
        public async Task<IActionResult> GetMyAuthorizationContracts()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("User not authenticated or invalid user ID");

            var contracts = await _authorizationContractRepository.GetAuthorizationContractsByUserAsync(userId);
            return Ok(contracts);
        }

        [HttpGet("authorization/{id}")]
        public async Task<IActionResult> GetAuthorizationContractById(int id)
        {
            var contract = await _authorizationContractRepository.GetAuthorizationContractByIdAsync(id);
            if (contract == null)
                return NotFound("Authorization contract not found");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("User not authenticated or invalid user ID");

            if (contract.CreatedById != userId)
                return Forbid("You are not authorized to view this contract");

            var contractDTO = new AuthorizationContractDTO
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                //Date = contract.Date,
                PartyAId = contract.PartyAId,
                PartyBId = contract.PartyBId,
                PdfUrl = contract.PdfUrl,
                CreatedById = contract.CreatedById,
                CreatedAt = contract.CreatedAt
            };

            return Ok(contractDTO);
        }
    }
}