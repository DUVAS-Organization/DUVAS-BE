using DataAccess;
using DTO;
using DUVAS;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDAO _dao;

        public MessageRepository(ApplicationDbContext context)
        {
            _dao = new MessageDAO(context);
        }

        public async Task AddMessageAsync(Message message) => await _dao.AddMessageAsync(message);
        public async Task<MessageDTO> GetMessageByIdAsync(int messageId) => await _dao.GetMessageByIdAsync(messageId);
        public async Task<List<MessageDTO>> GetMessagesByUserIdAsync(int userSendId, int userGetId) => await _dao.GetMessagesByUserIdAsync(userSendId, userGetId);
        public async Task UpdateMessageStatusAsync(int messageId, int status) => await _dao.UpdateMessageStatusAsync(messageId, status);
        public async Task DeleteMessageAsync(int messageId) => await _dao.DeleteMessageAsync(messageId);
        public async Task<List<ConversationDTO>> GetConversationsByUserIdAsync(int userId)
        {
            return await _dao.GetConversationsByUserIdAsync(userId);
        }
    }
}
