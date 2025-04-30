using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ServiceLicenseDAO
    {
        private readonly ApplicationDbContext _context;

        public ServiceLicenseDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceLicenseDTO>> GetServiceLicensesAsync()
        {
            try
            {
                var serviceLicenses = await _context.ServiceLicenses
                    .AsNoTracking()
                    .Select(p => new ServiceLicenseDTO
                    {
                        ServiceLicenseId = p.ServiceLicenseId,
                        UserId = p.UserId,
                        Name = p.Name,
                        dateOfBirth = p.dateOfBirth,
                        Sex = p.Sex,
                        Address = p.Address,
                        GiayPhepKinhDoanh = p.GiayPhepKinhDoanh,
                        GiayPhepChuyenMon = p.GiayPhepChuyenMon,
                        Status = p.Status
                    })
                    .ToListAsync();

                return serviceLicenses;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách ServiceLicense: " + ex.Message);
            }
        }

        public async Task<ServiceLicense> FindServiceLicenseByIdAsync(int serviceLicenseId)
        {
            try
            {
                return await _context.ServiceLicenses
                    .SingleOrDefaultAsync(x => x.ServiceLicenseId == serviceLicenseId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm ServiceLicense: " + ex.Message);
            }
        }

        public async Task SaveServiceLicenseAsync(ServiceLicense serviceLicense)
        {
            try
            {
                var existing = await _context.ServiceLicenses
                    .FirstOrDefaultAsync(l => l.UserId == serviceLicense.UserId);
                if (existing != null)
                {
                    throw new Exception("Người dùng đã có yêu cầu đang chờ xử lý.");
                }

                await _context.ServiceLicenses.AddAsync(serviceLicense);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lưu ServiceLicense: " + ex.Message);
            }
        }

        public async Task UpdateServiceLicenseAsync(ServiceLicense serviceLicense)
        {
            try
            {
                _context.Entry(serviceLicense).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật ServiceLicense: " + ex.Message);
            }
        }

        public async Task DeleteServiceLicenseAsync(ServiceLicense serviceLicense)
        {
            try
            {
                var existingServiceLicense = await _context.ServiceLicenses
                    .SingleOrDefaultAsync(c => c.ServiceLicenseId == serviceLicense.ServiceLicenseId);
                if (existingServiceLicense != null)
                {
                    _context.ServiceLicenses.Remove(existingServiceLicense);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa ServiceLicense: " + ex.Message);
            }
        }

        public async Task<bool> IsCCCDExistsAsync(string cccd)
        {
            try
            {
                return await _context.ServiceLicenses.AnyAsync(s => s.CCCD == cccd);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra CCCD: " + ex.Message);
            }
        }

        public async Task<ServiceLicense?> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.ServiceLicenses
                    .SingleOrDefaultAsync(s => s.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm ServiceLicense theo UserId: " + ex.Message);
            }
        }
    }
}