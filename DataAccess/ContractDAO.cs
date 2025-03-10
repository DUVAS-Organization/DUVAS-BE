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
                    var contracts = await context.Contracts
                        .AsNoTracking()
                        .Select(p => new ContractDTO
                        {
                            ContractId = p.ContractId,
                            RentalDateTimeStart = p.RentalDateTimeStart,
                            RentalDateTimeEnd = p.RentalDateTimeEnd,
                            ContractFile = p.ContractFile,
                            Status = p.status, // Trạng thái hợp đồng\
                            RoomId = 0
                        })
                        .ToListAsync();

                    return contracts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hợp đồng: " + ex.Message);
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

        public static async Task<int> NewContractAsync(Contract contract)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.Contracts.AddAsync(contract);
                    await context.SaveChangesAsync();
                    return contract.ContractId; // Trả về ID của hợp đồng sau khi lưu thành công
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lưu hợp đồng: " + ex.Message);
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

        // Cập nhật trạng thái hợp đồng (Xác nhận hoặc Hủy hợp đồng)
        public static async Task UpdateContractStatusAsync(int contractId, int status)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contract = await context.Contracts.SingleOrDefaultAsync(c => c.ContractId == contractId);
                    if (contract != null)
                    {
                        contract.status = status; // 1: Đã xác nhận, 2: Đã hủy
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái hợp đồng: " + ex.Message);
            }
        }
    }
}
