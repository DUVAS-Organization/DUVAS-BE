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
    public class ContractDAO
    {
        private readonly ApplicationDbContext _context;

        public ContractDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<ContractDTO>> GetContractsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contract = await context.Contracts
                        .AsNoTracking()
                        .Select(p => new ContractDTO
                        {
                            ContractId = p.ContractId,
                            RentalDateTimeStart = p.RentalDateTimeStart,
                            RentalDateTimeEnd = p.RentalDateTimeEnd,
                            ContractFile = p.ContractFile,
                        })
                        .ToListAsync();

                    return contract;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static async Task<Contract> FindContractByIdAsync(int contractId)
        {
            Contract contract = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    contract = await context.Contracts.SingleOrDefaultAsync(x => x.ContractId == contractId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return contract;
        }

        public static async Task SaveContractAsync(Contract contract)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.Contracts.AddAsync(contract);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateContractAsync(Contract contract)
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

        public static async Task DeleteContractAsync(Contract contract)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingContract = await context.Contracts.SingleOrDefaultAsync(c => c.ContractId == contract.ContractId);
                    if (existingContract != null)
                    {
                        context.Contracts.Remove(existingContract);
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
