using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class LandlordLicenseDAO
    {
        private readonly ApplicationDbContext _context;

        public LandlordLicenseDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LandlordLicenseDTO>> GetLandlordLicensesAsync()
        {
            try
            {
                var landlordLicenses = await _context.LandlordLicenses
                    .AsNoTracking()
                    .Select(p => new LandlordLicenseDTO
                    {
                        LandlordLicenseId = p.LandlordLicenseId,
                        UserId = p.UserId,
                        Name = p.Name,
                        dateOfBirth = p.dateOfBirth,
                        Sex = p.Sex,
                        Address = p.Address,
                        GiayPhepKinhDoanh = p.GiayPhepKinhDoanh,
                        Status = p.Status
                    })
                    .ToListAsync();
                return landlordLicenses;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách LandlordLicense: " + ex.Message);
            }
        }

        public async Task<LandlordLicense> FindLandlordLicenseByIdAsync(int landlordLicenseId)
        {
            try
            {
                return await _context.LandlordLicenses
                    .SingleOrDefaultAsync(x => x.LandlordLicenseId == landlordLicenseId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm LandlordLicense: " + ex.Message);
            }
        }

        public async Task SaveLandlordLicenseAsync(LandlordLicense landlordLicense)
        {
            try
            {
                var existing = await _context.LandlordLicenses
                    .FirstOrDefaultAsync(l => l.UserId == landlordLicense.UserId);
                if (existing != null)
                {
                    throw new Exception("Người dùng đã có yêu cầu đang chờ xử lý.");
                }

                await _context.LandlordLicenses.AddAsync(landlordLicense);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lưu LandlordLicense: " + ex.Message);
            }
        }

        public async Task UpdateLandlordLicenseAsync(LandlordLicense landlordLicense)
        {
            try
            {
                _context.Entry(landlordLicense).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật LandlordLicense: " + ex.Message);
            }
        }

        public async Task DeleteLandlordLicenseAsync(LandlordLicense landlordLicense)
        {
            try
            {
                var existingLandlordLicense = await _context.LandlordLicenses
                    .SingleOrDefaultAsync(c => c.LandlordLicenseId == landlordLicense.LandlordLicenseId);
                if (existingLandlordLicense != null)
                {
                    _context.LandlordLicenses.Remove(existingLandlordLicense);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa LandlordLicense: " + ex.Message);
            }
        }

        public async Task<bool> IsCCCDExistsAsync(string cccd)
        {
            try
            {
                return await _context.LandlordLicenses.AnyAsync(l => l.CCCD == cccd);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra CCCD: " + ex.Message);
            }
        }

        public async Task<LandlordLicense?> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.LandlordLicenses
                    .SingleOrDefaultAsync(l => l.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm LandlordLicense theo UserId: " + ex.Message);
            }
        }
    }
}