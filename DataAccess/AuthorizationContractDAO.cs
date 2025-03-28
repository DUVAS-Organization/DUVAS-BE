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
                            CreatedAt = p.CreatedAt
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
                            CreatedAt = p.CreatedAt
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
                    await context.AuthorizationContracts.AddAsync(contract);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
    }
}