using DTO;
using DUVAS;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using API.Hubs;
using BusinessObject;
using DataAccess;

namespace API.Controllers.UserAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(IMessageRepository messageRepository, IHubContext<ChatHub> hubContext)
        {
            _messageRepository = messageRepository;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            message.DateTime = DateTime.UtcNow;
            message.Status = 0;

            await _messageRepository.AddMessageAsync(message);

            // Gửi tin nhắn qua SignalR đến người gửi và người nhận
            await _hubContext.Clients.Group($"user-{message.UserGetID}")
                .SendAsync("ReceiveMessage", message);
            await _hubContext.Clients.Group($"user-{message.UserSendID}")
                .SendAsync("ReceiveMessage", message);

            // Gửi thông báo
            var messages = $"Bạn vừa có tin nhắn mới từ #{message.UserSendID}";
            var redirectUrl = $"";
            await NotificationDAO.CreateNotificationAsync(new Notification
            {
                UserId = message.UserGetID,
                Type = "message",
                Message = messages,
                RedirectUrl = redirectUrl,
                CreatedDate = DateTime.UtcNow,
                IsRead = false
            });

            return Ok(message);
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
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            await _messageRepository.UpdateMessageStatusAsync(messageId, status);

            if (status == 1 && message != null)
            {
                var unreadCount = await _messageRepository.GetUnreadCountAsync(message.UserGetID);
                await _hubContext.Clients.Group($"user-{message.UserGetID}")
                    .SendAsync("UpdateUnreadCount", unreadCount);

                await UpdateConversationList(message.UserGetID);
            }

            return Ok();
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            await _messageRepository.DeleteMessageAsync(messageId);
            return Ok();
        }

        [HttpGet("conversations/{userId}")]
        public async Task<IActionResult> GetConversationsByUserId(int userId)
        {
            var conversations = await _messageRepository.GetConversationsByUserIdAsync(userId);
            return Ok(conversations);
        }

        [HttpGet("unread/{userId}")]
        public async Task<IActionResult> GetUnreadCount(int userId)
        {
            var count = await _messageRepository.GetUnreadCountAsync(userId);
            return Ok(new UnreadCountDTO { Count = count });
        }

        private async Task UpdateConversationList(int userId)
        {
            var conversations = await _messageRepository.GetConversationsByUserIdAsync(userId);
            await _hubContext.Clients.Group($"user-{userId}")
                .SendAsync("UpdateConversations", conversations);
        }
    }
}