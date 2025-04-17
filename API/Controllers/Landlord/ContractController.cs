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
using DUVAS;
using DataAccess;

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
        private readonly IRoomRepository _roomRepository;

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
            // Validate input
            if (details == null || details.PartyAId <= 0 || details.PartyBId <= 0)
            {
                return BadRequest("Invalid contract details.");
            }

            // Tạo file PDF
            var pdfBytes = _pdfService.GenerateAuthorizationContractPdf(details);

            // Upload lên Cloudinary
            var fileName = $"authorization_contract_{details.PartyAId}_{DateTime.Now.Ticks}.pdf";
            var pdfUrl = await _cloudinaryService.UploadPdfAsync(pdfBytes, fileName);

            // Lưu thông tin cơ bản vào DB
            var contract = new AuthorizationContract
            {
                ContractNumber = details.ContractNumber,
                Date = details.Date,
                PartyAId = details.PartyAId,
                PartyBId = details.PartyBId,
                PdfUrl = pdfUrl,
                CreatedById = details.PartyAId,
                CreatedAt = DateTime.UtcNow,
                status = 2,
            };

            await _authorizationContractRepository.SaveAuthorizationContractAsync(contract);

            return Ok(new { ContractId = contract.Id, PdfUrl = pdfUrl });
        }
        [HttpGet("all-authorization-contract")]
        public async Task<IActionResult> GetAllAuthorizationContracts()
        {
            try
            {
                var contracts = await AuthorizationContractDAO.GetAuthorizationContractsAsync();
                if (contracts == null || !contracts.Any())
                {
                    return NotFound("Không tìm thấy hợp đồng ủy quyền nào.");
                }
                return Ok(contracts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách hợp đồng ủy quyền: {ex.Message}");
            }
        }
        [HttpGet("my-authorization-contracts")]
        public async Task<IActionResult> GetMyAuthorizationContracts(int userId)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            //    return Unauthorized("User not authenticated or invalid user ID");

            var contracts = await _authorizationContractRepository.GetAuthorizationContractsByUserAsync(userId);
            return Ok(contracts);
        }

        [HttpGet("authorization/{id}")]
        public async Task<IActionResult> GetAuthorizationContractById(int id)
        {
            var contract = await _authorizationContractRepository.GetAuthorizationContractByIdAsync(id);
            if (contract == null)
                return NotFound("Authorization contract not found");

            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            //    return Unauthorized("User not authenticated or invalid user ID");

            //if (contract.CreatedById != userId)
            //    return Forbid("You are not authorized to view this contract");

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

        [HttpPut("update-rooms-authorization")]
        public async Task<IActionResult> UpdateRoomsAuthorization([FromBody] UpdateRoomsAuthorizationRequest request)
        {
            try
            {
                // Kiểm tra xác thực người dùng (nếu cần)
                //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                //    return Unauthorized("User not authenticated or invalid user ID");

                // Kiểm tra danh sách roomIds hợp lệ
                if (request.RoomIds == null || !request.RoomIds.Any())
                    return BadRequest("Danh sách RoomIds không được để trống.");

                // Cập nhật Authorization cho từng phòng
                foreach (var roomId in request.RoomIds)
                {
                    // Kiểm tra quyền của người dùng đối với phòng (nếu cần)
                    //var room = await _roomRepository.GetRoomEntityByIdForLandlordAsync(roomId, userId);
                    //if (room == null)
                    //    return Forbid($"Bạn không có quyền cập nhật phòng với ID {roomId} hoặc phòng không tồn tại.");

                    await _roomRepository.UpdateAuthorizationAsync(roomId, request.Authorization);
                }

                return Ok(new { Message = "Cập nhật Authorization cho các phòng thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Lỗi khi cập nhật Authorization: {ex.Message}" });
            }
        }

        [HttpPut("update-contracts-status")]
        public async Task<IActionResult> UpdateContractsStatus([FromBody] UpdateContractsStatusRequest request)
        {
            try
            {
                // Kiểm tra xác thực người dùng
                //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                //    return Unauthorized("User not authenticated or invalid user ID");

                // Kiểm tra danh sách contractIds hợp lệ
                if (request.ContractIds == null || !request.ContractIds.Any())
                    return BadRequest("Danh sách ContractIds không được để trống.");

                // Cập nhật status cho từng hợp đồng
                foreach (var contractId in request.ContractIds)
                {
                    // Kiểm tra quyền của người dùng đối với hợp đồng
                    var contract = await _authorizationContractRepository.GetAuthorizationContractByIdAsync(contractId);
                    if (contract == null)
                        return NotFound($"Hợp đồng với ID {contractId} không tồn tại.");
                    //if (contract.CreatedById != userId)
                    //    return Forbid($"Bạn không có quyền cập nhật hợp đồng với ID {contractId}.");

                    await _authorizationContractRepository.UpdateStatusAsync(contractId, request.Status);
                }

                return Ok(new { Message = "Cập nhật status cho các hợp đồng thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Lỗi khi cập nhật status: {ex.Message}" });
            }
        }
    }
}