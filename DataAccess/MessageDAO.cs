using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MessageDAO
    {
        //private readonly ApplicationDbContext _context;

        //public MessageDAO(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        //public static async Task<List<MessageDTO>> GetMessagesAsync()
        //{
        //    try
        //    {
        //        using (var context = new ApplicationDbContext())
        //        {
        //            var contract = await context.Messages
        //                .AsNoTracking()
        //                .Select(p => new MessageDTO
        //                {
        //                    MessageId = p.MessageId,
        //                    Content = p.Content,
        //                })
        //                .ToListAsync();

        //            return contract;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        //public static async Task<Message> FindMessageByIdAsync(int messageId)
        //{
        //    Message message = null;
        //    try
        //    {
        //        using (var context = new ApplicationDbContext())
        //        {
        //            message = await context.Messages.SingleOrDefaultAsync(x => x.MessageId == messageId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return message;
        //}

        //public static async Task SaveMessageAsync(Message message)
        //{
        //    try
        //    {
        //        using (var context = new ApplicationDbContext())
        //        {
        //            await context.Messages.AddAsync(message);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task UpdateMessageAsync(Message message)
        //{
        //    try
        //    {
        //        using (var context = new ApplicationDbContext())
        //        {
        //            context.Entry(message).State = EntityState.Modified;
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static async Task DeleteContractAsync(Message message)
        //{
        //    try
        //    {
        //        using (var context = new ApplicationDbContext())
        //        {
        //            var existingMessage = await context.Messages.SingleOrDefaultAsync(c => c.MessageId == message.MessageId);
        //            if (existingMessage != null)
        //            {
        //                context.Messages.Remove(existingMessage);
        //                await context.SaveChangesAsync();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
