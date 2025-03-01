using DTO;
using DUVAS;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            await _messageRepository.AddMessageAsync(message);
            return Ok();
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetMessageById(int messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            return message != null ? Ok(message) : NotFound();
        }

        [HttpGet("user/{userSendId}/{userGetId}")]
        public async Task<IActionResult> GetMessagesByUserId(int userSendId, int userGetId)
        {
            var messages = await _messageRepository.GetMessagesByUserIdAsync(userSendId, userGetId);
            return Ok(messages);
        }


        [HttpPut("{messageId}/status/{status}")]
        public async Task<IActionResult> UpdateMessageStatus(int messageId, int status)
        {
            await _messageRepository.UpdateMessageStatusAsync(messageId, status);
            return Ok();
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            await _messageRepository.DeleteMessageAsync(messageId);
            return Ok();
        }
    }
}
