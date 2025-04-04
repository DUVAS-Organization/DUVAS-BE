using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class InsiderTradingDAO
    {
        private readonly ApplicationDbContext _context;

        public InsiderTradingDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public static async Task<List<InsiderTradingDTO>> GetInsiderTradingsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.InsiderTradings.AsNoTracking()
                        .Select(p => new InsiderTradingDTO
                        {
                            InsiderTradingId = p.InsiderTradingId,
                            Remitter = p.Remitter,
                            Receiver = p.Receiver,
                            Money = p.Money,
                            Note = p.Note,
                            RoomId = p.RoomId,
                            PriorityPackageRoomId = p.PriorityPackageRoomId,
                            Status = p.Status,
                            Type = p.Type,
                            CreatedDate = p.CreatedDate,
                            HoldUntil = p.HoldUntil
                        })
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<InsiderTradingDTO> FindInsiderTradingByIdAsync(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var insiderTrading = await context.InsiderTradings.SingleOrDefaultAsync(x => x.InsiderTradingId == id);

                    return insiderTrading == null ? null : new InsiderTradingDTO
                    {
                        InsiderTradingId = insiderTrading.InsiderTradingId,
                        Remitter = insiderTrading.Remitter,
                        Receiver = insiderTrading.Receiver,
                        Money = insiderTrading.Money,
                        Note = insiderTrading.Note,
                        RoomId = insiderTrading.RoomId,
                        PriorityPackageRoomId = insiderTrading.PriorityPackageRoomId,
                        Status = insiderTrading.Status,
                        Type = insiderTrading.Type,
                        CreatedDate = insiderTrading.CreatedDate,
                        HoldUntil = insiderTrading.HoldUntil
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<int> SaveInsiderTradingAsync(InsiderTradingDTO insiderTradingDTO, string type)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var insiderTrading = new InsiderTrading
                    {
                        //InsiderTradingId = insiderTradingDTO.InsiderTradingId,
                        Remitter = insiderTradingDTO.Remitter,
                        Receiver = insiderTradingDTO.Receiver,
                        Money = insiderTradingDTO.Money,
                        Note = $"User ID {insiderTradingDTO.Remitter} vừa chuyển {insiderTradingDTO.Money} đến User ID {insiderTradingDTO.Receiver}.",
                        RoomId = insiderTradingDTO.RoomId,
                        PriorityPackageRoomId = insiderTradingDTO.PriorityPackageRoomId,
                        Status = insiderTradingDTO.Status,
                        Type = type,
                        CreatedDate = insiderTradingDTO.CreatedDate,
                        HoldUntil = insiderTradingDTO.HoldUntil
                    };
                    await context.InsiderTradings.AddAsync(insiderTrading);
                    await context.SaveChangesAsync();
                    return insiderTrading.InsiderTradingId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateInsiderTradingAsync(InsiderTradingDTO insiderTradingDTO)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var insiderTrading = new InsiderTrading
                    {
                        InsiderTradingId = insiderTradingDTO.InsiderTradingId,
                        Remitter = insiderTradingDTO.Remitter,
                        Receiver = insiderTradingDTO.Receiver,
                        Money = insiderTradingDTO.Money,
                        Note = insiderTradingDTO.Note,
                        RoomId = insiderTradingDTO.RoomId,
                        PriorityPackageRoomId = insiderTradingDTO.PriorityPackageRoomId,
                        Status = insiderTradingDTO.Status,
                        Type = insiderTradingDTO.Type,
                        CreatedDate = insiderTradingDTO.CreatedDate,
                        HoldUntil = insiderTradingDTO.HoldUntil
                    };
                    context.Entry(insiderTrading).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteInsiderTradingAsync(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existing = await context.InsiderTradings.SingleOrDefaultAsync(c => c.InsiderTradingId == id);
                    if (existing != null)
                    {
                        context.InsiderTradings.Remove(existing);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task UpdateInsiderTradingStatusAsync(int id, int status)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var insiderTrading = await context.InsiderTradings.SingleOrDefaultAsync(x => x.InsiderTradingId == id);
                    if (insiderTrading != null)
                    {
                        insiderTrading.Status = status;
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}