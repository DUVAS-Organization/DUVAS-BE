using DTO;
using DUVAS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<MessageDTO> GetMessageByIdAsync(int messageId);
        Task<List<MessageDTO>> GetMessagesByUserIdAsync(int userSendId, int userGetId);
        Task UpdateMessageStatusAsync(int messageId, int status);
        Task DeleteMessageAsync(int messageId);
    }
}
