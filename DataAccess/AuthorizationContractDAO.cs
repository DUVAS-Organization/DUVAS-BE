using BusinessObject;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;

namespace DataAccess
{
    public class AuthorizationContractDAO
    {
        public static async Task<List<AuthorizationContractDTO>> GetAuthorizationContractsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contracts = await context.AuthorizationContracts
                        .AsNoTracking()
                        .Select(p => new AuthorizationContractDTO
                        {
                            Id = p.Id,
                            ContractNumber = p.ContractNumber,
                            Date = p.Date,
                            PartyAId = p.PartyAId,
                            PartyBId = p.PartyBId,
                            PdfUrl = p.PdfUrl,
                            CreatedById = p.CreatedById,
                            CreatedAt = p.CreatedAt,
                            status = p.status
                        })
                        .ToListAsync();

                    return contracts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<AuthorizationContractDTO>> GetAuthorizationContractsByUserAsync(int createdById)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contracts = await context.AuthorizationContracts
                        .AsNoTracking()
                        .Where(c => c.CreatedById == createdById)
                        .Select(p => new AuthorizationContractDTO
                        {
                            Id = p.Id,
                            ContractNumber = p.ContractNumber,
                            Date = p.Date,
                            PartyAId = p.PartyAId,
                            PartyBId = p.PartyBId,
                            PdfUrl = p.PdfUrl,
                            CreatedById = p.CreatedById,
                            CreatedAt = p.CreatedAt,
                            status = p.status
                        })
                        .ToListAsync();

                    return contracts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<AuthorizationContract> FindAuthorizationContractByIdAsync(int id)
        {
            AuthorizationContract contract = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    contract = await context.AuthorizationContracts
                        .SingleOrDefaultAsync(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return contract;
        }

        public static async Task SaveAuthorizationContractAsync(AuthorizationContract contract)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    // Tìm các phòng do PartyA sở hữu
                    var relatedRooms = await context.Rooms
                        .Where(r => r.UserId == contract.PartyAId)
                        .ToListAsync();

                    // Set Authorization = 2 cho các phòng đó
                    foreach (var room in relatedRooms)
                    {
                        room.Authorization = 2;
                    }

                    await context.AuthorizationContracts.AddAsync(contract);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu hợp đồng và cập nhật phòng: {ex.Message}");
            }
        }


        public static async Task UpdateAuthorizationContractAsync(AuthorizationContract contract)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(contract).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteAuthorizationContractAsync(AuthorizationContract contract)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingContract = await context.AuthorizationContracts
                        .SingleOrDefaultAsync(c => c.Id == contract.Id);
                    if (existingContract != null)
                    {
                        context.AuthorizationContracts.Remove(existingContract);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task UpdateStatusAsync(int id, int status)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contract = await context.AuthorizationContracts.FirstOrDefaultAsync(c => c.Id == id);
                    if (contract == null)
                    {
                        throw new KeyNotFoundException($"Hợp đồng ủy quyền với ID {id} không tồn tại.");
                    }

                    contract.status = status;
                    context.AuthorizationContracts.Update(contract);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật status: {ex.Message}");
            }
        }
    }
}