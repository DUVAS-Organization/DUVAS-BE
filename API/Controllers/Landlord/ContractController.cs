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
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IAuthorizationContractRepository _authorizationContractRepository;
        private readonly PdfService _pdfService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly IRoomRepository _roomRepository;

        public ContractController(
            IAuthorizationContractRepository authorizationContractRepository,
            PdfService pdfService,
            CloudinaryService cloudinaryService,
            IRoomRepository roomRepository)
        {
            _authorizationContractRepository = authorizationContractRepository;
            _pdfService = pdfService;
            _cloudinaryService = cloudinaryService;
            _roomRepository = roomRepository;
        }

        [HttpPost("generate-authorization")]
        public async Task<IActionResult> GenerateAuthorizationContract([FromBody] PdfService.AuthorizationContractDetails details)
        {
            // Validate input
            if (details == null || details.PartyAId <= 0 || details.PartyBId <= 0)
            {
                return BadRequest("Invalid contract details.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Validate SelectedRoom (nếu cần)
            //if (details.SelectedRoom == null || !details.SelectedRoom.Any())
            //{
            //    return BadRequest("No rooms selected.");
            //}
            var roomListString = string.Join(",", details.SelectedRoom);

            // Tạo file PDF
            var pdfBytes = _pdfService.GenerateAuthorizationContractPdf(details);

            // Upload lên Cloudinary
            var fileName = $"authorization_contract_{details.PartyAId}_{DateTime.UtcNow.Ticks}.pdf";
            var pdfUrl = await _cloudinaryService.UploadPdfAsync(pdfBytes, fileName);

            // Lưu thông tin cơ bản vào DB
            var contract = new AuthorizationContract
            {
                ContractNumber = details.ContractNumber,
                //Date = details.Date,
                PartyAId = details.PartyAId,
                PartyBId = details.PartyBId,
                PdfUrl = pdfUrl,
                CreatedById = details.PartyAId,
                CreatedAt = DateTime.UtcNow,
                status = 2,
                RoomList = roomListString
            };

            await _authorizationContractRepository.SaveAuthorizationContractAsync(contract);

            return Ok(new { ContractId = contract.Id, PdfUrl = pdfUrl });
        }

       

        [HttpGet("authorization")]
        public async Task<IActionResult> GetAllAuthorizationContract()
        {
            var contract = await _authorizationContractRepository.GetAuthorizationContractsAsync();
            if (contract == null)
                return NotFound("Không có Author Contract nào");
            return Ok(contract);
        }

        [HttpGet("my-authorization-contracts")]
        public async Task<IActionResult> GetMyAuthorizationContracts()
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            //    return Unauthorized("User not authenticated or invalid user ID");
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return BadRequest("UserId claim not found.");
            }
            int.TryParse(userIdClaim.Value, out int userId);
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
                CreatedAt = contract.CreatedAt,
                status = contract.status,
                RoomList = contract.RoomList,
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
                if (request == null)
                {
                    return BadRequest("Request body không hợp lệ.");
                }
                // Kiểm tra danh sách roomIds hợp lệ
                if (request.RoomIds == null || !request.RoomIds.Any())
                    return BadRequest("Danh sách RoomIds không được để trống.");
                
                // Cập nhật Authorization cho từng phòng
                foreach (var roomId in request.RoomIds)
                {
                    // Kiểm tra quyền của người dùng đối với phòng (nếu cần)
                   

                    await _roomRepository.UpdateAuthorizationAsync(roomId, request.Authorization);
                }

                return Ok(new { Message = "Cập nhật Authorization cho các phòng thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateRoomsAuthorization: {ex.Message}\n{ex.StackTrace}");
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