using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MessageDAO
    {
        private readonly ApplicationDbContext _context;

        public MessageDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MessageDTO>> GetMessagesByUserIdAsync(int userSendId, int userGetId)
        {
            try
            {
                return await _context.Messages
                    .AsNoTracking()
                    .Where(m => (m.UserSendID == userSendId && m.UserGetID == userGetId) || (m.UserSendID == userGetId && m.UserGetID == userSendId))
                    .Include(m => m.UserSend)
                    .Select(m => new MessageDTO
                    {
                        MessageId = m.MessageId,
                        UserSendID = m.UserSendID,
                        UserGetID = m.UserGetID.ToString(),
                        Content = m.Content,
                        Image = m.Image,
                        DateTime = m.DateTime,
                        Status = m.Status
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MessageDTO> GetMessageByIdAsync(int messageId)
        {
            try
            {
                return await _context.Messages
                    .AsNoTracking()
                    .Where(m => m.MessageId == messageId)
                    .Select(m => new MessageDTO
                    {
                        MessageId = m.MessageId,
                        UserSendID = m.UserSendID,
                        UserGetID = m.UserGetID.ToString(),
                        Content = m.Content,
                        Image = m.Image,
                        DateTime = m.DateTime,
                        Status = m.Status
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddMessageAsync(Message message)
        {
            try
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateMessageStatusAsync(int messageId, int status)
        {
            try
            {
                var message = await _context.Messages.FindAsync(messageId);
                if (message != null)
                {
                    message.Status = status;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            try
            {
                var message = await _context.Messages.FindAsync(messageId);
                if (message != null)
                {
                    _context.Messages.Remove(message);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
