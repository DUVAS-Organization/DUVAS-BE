using BusinessObject;
using DTO;
using DUVAS;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoomDAO
    {
        private readonly ApplicationDbContext _context;

        public RoomDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<RoomDTO>> GetRoomsAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var rooms = await context.Rooms
                         .Include(r => r.PriorityPackageRooms)
                        .AsNoTracking()
                        .Select(p => new RoomDTO
                        {
                            RoomId = p.RoomId,
                            BuildingId = p.BuildingId,
                            UserId = p.UserId,
                            UserName = p.User.UserName,
                            Title = p.Title,
                            Description = p.Description,
                            LocationDetail = p.LocationDetail,
                            Acreage = p.Acreage,
                            Furniture = p.Furniture,
                            NumberOfBathroom = p.NumberOfBathroom,
                            NumberOfBedroom = p.NumberOfBedroom,
                            Garret = p.Garret,
                            Price = p.Price,
                            CategoryRoomId = p.CategoryRoomId,
                            Image = p.Image,
                            Note = p.Note,
                            status = p.status,
                            reputation = p.reputation,
                            Deposit = p.Deposit,
                            Dien = p.Dien,
                            Nuoc = p.Nuoc,
                            Internet = p.Internet,
                            Rac = p.Rac,
                            GuiXe = p.GuiXe,
                            QuanLy = p.QuanLy,
                            ChiPhiKhac = p.ChiPhiKhac,
                            BuildingName = p.Building != null ? p.Building.BuildingName : null,
                            CategoryName = p.CategoryRoom.CategoryName,
                            IsPermission = p.IsPermission,
                            Authorization = p.Authorization,
                            PriorityPackageRooms = p.PriorityPackageRooms.Select(pr => new PriorityPackageRoomDTO
                            {
                                PriorityPackageRoomId = pr.PriorityPackageRoomId,
                                RoomId = pr.RoomId,
                                CategoryPriorityPackageRoomId = pr.CategoryPriorityPackageRoomId,
                                StartDate = pr.StartDate,
                                EndDate = pr.EndDate,
                                Price = pr.Price,
                                UserId = pr.UserId,
                            }).ToList()
                        })
                        .ToListAsync();


                    return rooms;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static async Task<List<RoomDTO>> GetListRoomLockAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.Rooms
                    .Where(p => p.IsPermission == 0)
                    .AsNoTracking()
                    .Select(p => new RoomDTO
                    {
                        RoomId = p.RoomId,
                        BuildingId = p.BuildingId,
                        UserId = p.UserId,
                        UserName = p.User.UserName,
                        Title = p.Title,
                        Description = p.Description,
                        LocationDetail = p.LocationDetail,
                        Acreage = p.Acreage,
                        Furniture = p.Furniture,
                        NumberOfBathroom = p.NumberOfBathroom,
                        NumberOfBedroom = p.NumberOfBedroom,
                        Garret = p.Garret,
                        Price = p.Price,
                        CategoryRoomId = p.CategoryRoomId,
                        Image = p.Image,
                        Note = p.Note,
                        Dien = p.Dien,
                        Nuoc = p.Nuoc,
                        Internet = p.Internet,
                        Rac = p.Rac,
                        GuiXe = p.GuiXe,
                        QuanLy = p.QuanLy,
                        ChiPhiKhac = p.ChiPhiKhac,
                        BuildingName = p.Building != null ? p.Building.BuildingName : null,
                        CategoryName = p.CategoryRoom.CategoryName,
                        IsPermission = p.IsPermission,
                        status = p.status,
                        Authorization = p.Authorization,
                    })
                    .ToListAsync();
            }
        }
        public static async Task<List<RoomDTO>> GetListRoomActiveAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.Rooms
                    .Where(p => p.IsPermission == 1)
                    .AsNoTracking()
                    .Select(p => new RoomDTO
                    {
                        RoomId = p.RoomId,
                        BuildingId = p.BuildingId,
                        UserId = p.UserId,
                        UserName = p.User.UserName,
                        Title = p.Title,
                        Description = p.Description,
                        LocationDetail = p.LocationDetail,
                        Acreage = p.Acreage,
                        Furniture = p.Furniture,
                        NumberOfBathroom = p.NumberOfBathroom,
                        NumberOfBedroom = p.NumberOfBedroom,
                        Garret = p.Garret,
                        Price = p.Price,
                        CategoryRoomId = p.CategoryRoomId,
                        Image = p.Image,
                        Note = p.Note,
                        Dien = p.Dien,
                        Nuoc = p.Nuoc,
                        Internet = p.Internet,
                        Rac = p.Rac,
                        GuiXe = p.GuiXe,
                        QuanLy = p.QuanLy,
                        ChiPhiKhac = p.ChiPhiKhac,
                        BuildingName = p.Building != null ? p.Building.BuildingName : null,
                        CategoryName = p.CategoryRoom.CategoryName,
                        IsPermission = p.IsPermission,
                        status = p.status,
                        Authorization = p.Authorization,
                    })
                    .ToListAsync();
            }
        }
        public static async Task<List<RoomDTO>> GetRoomReputationAsync()
        {
            using var context = new ApplicationDbContext();
            var rooms = await context.Rooms
                .Where(r => context.RoomLicenses
                    .Any(rl => rl.RoomId == r.RoomId && !string.IsNullOrEmpty(rl.BienBanPCCC)))
                .Select(r => new RoomDTO
                {
                    RoomId = r.RoomId,
                    BuildingId = r.BuildingId,
                    UserId = r.UserId,
                    UserName = r.User.UserName,
                    Title = r.Title,
                    Description = r.Description,
                    LocationDetail = r.LocationDetail,
                    Acreage = r.Acreage,
                    Furniture = r.Furniture,
                    NumberOfBathroom = r.NumberOfBathroom,
                    NumberOfBedroom = r.NumberOfBedroom,
                    Garret = r.Garret,
                    Price = r.Price,
                    CategoryRoomId = r.CategoryRoomId,
                    Image = r.Image,
                    Note = r.Note,
                    status =r.status,
                    reputation = r.reputation,
                    Deposit = r.Deposit,
                    Dien = r.Dien,
                    Nuoc = r.Nuoc,
                    Internet = r.Internet,
                    Rac = r.Rac,
                    GuiXe = r.GuiXe,
                    QuanLy = r.QuanLy,
                    ChiPhiKhac = r.ChiPhiKhac,
                    BuildingName = r.Building != null ? r.Building.BuildingName : null,
                    CategoryName = r.CategoryRoom.CategoryName,
                    IsPermission = r.IsPermission,
                    Authorization = r.Authorization,
                })
                .ToListAsync();

            return rooms;
        }
        public static async Task<List<RoomDTO>> GetRoomsByLandlordAsync(int userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.Rooms
                     .Include(r => r.PriorityPackageRooms)
                    .Where(r => r.UserId == userId)
                    .AsNoTracking()
                    .Select(p => new RoomDTO
                    {
                        RoomId = p.RoomId,
                        BuildingId = p.BuildingId,
                        UserId = p.UserId,
                        UserName = p.User.UserName,
                        Title = p.Title,
                        Description = p.Description,
                        LocationDetail = p.LocationDetail,
                        Acreage = p.Acreage,
                        Furniture = p.Furniture,
                        NumberOfBathroom = p.NumberOfBathroom,
                        NumberOfBedroom = p.NumberOfBedroom,
                        Garret = p.Garret,
                        Price = p.Price,
                        CategoryRoomId = p.CategoryRoomId,
                        Image = p.Image,
                        Note = p.Note,
                        status = p.status,
                        reputation = p.reputation,
                        Deposit = p.Deposit,
                        Dien = p.Dien,
                        Nuoc = p.Nuoc,
                        Internet = p.Internet,
                        Rac = p.Rac,
                        GuiXe = p.GuiXe,
                        QuanLy = p.QuanLy,
                        ChiPhiKhac = p.ChiPhiKhac,
                        BuildingName = p.Building != null ? p.Building.BuildingName : null,
                        CategoryName = p.CategoryRoom.CategoryName,
                        IsPermission = p.IsPermission,
                        Authorization = p.Authorization,
                        PriorityPackageRooms = p.PriorityPackageRooms.Select(pr => new PriorityPackageRoomDTO
                        {
                            PriorityPackageRoomId = pr.PriorityPackageRoomId,
                            RoomId = pr.RoomId,
                            CategoryPriorityPackageRoomId = pr.CategoryPriorityPackageRoomId,
                            StartDate = pr.StartDate,
                            EndDate = pr.EndDate,
                            Price = pr.Price,
                            UserId = pr.UserId,
                        }).ToList()
                    })
                    .ToListAsync();
            }
        }
        public static async Task<RoomDTO> GetRoomByIdForLandlordAsync(int roomId, int landlordId)
        {
            using (var context = new ApplicationDbContext())
            {

                List<Room> room = context.Rooms.Where(r => r.RoomId == roomId && r.UserId == landlordId).ToList();

                return await context.Rooms
                    .Include(r => r.PriorityPackageRooms)
                    .Where(r => r.RoomId == roomId && r.UserId == landlordId) // Lọc theo RoomId và LandlordId
                    .AsNoTracking()
                    .Select(p => new RoomDTO
                    {
                        RoomId = p.RoomId,
                        BuildingId = p.BuildingId,
                        UserId = p.UserId,
                        UserName = p.User.UserName,
                        Title = p.Title,
                        Description = p.Description,
                        LocationDetail = p.LocationDetail,
                        Acreage = p.Acreage,
                        Furniture = p.Furniture,
                        NumberOfBathroom = p.NumberOfBathroom,
                        NumberOfBedroom = p.NumberOfBedroom,
                        Garret = p.Garret,
                        Price = p.Price,
                        CategoryRoomId = p.CategoryRoomId,
                        Image = p.Image,
                        Note = p.Note,
                        status = p.status,
                        reputation = p.reputation,
                        Deposit = p.Deposit,
                        Dien = p.Dien,
                        Nuoc = p.Nuoc,
                        Internet = p.Internet,
                        Rac = p.Rac,
                        GuiXe = p.GuiXe,
                        QuanLy = p.QuanLy,
                        ChiPhiKhac = p.ChiPhiKhac,
                        BuildingName = p.Building != null ? p.Building.BuildingName : null,
                        CategoryName = p.CategoryRoom.CategoryName,
                        IsPermission = p.IsPermission,
                        Authorization = p.Authorization,
                        PriorityPackageRooms = p.PriorityPackageRooms.Select(pr => new PriorityPackageRoomDTO
                        {
                            PriorityPackageRoomId = pr.PriorityPackageRoomId,
                            RoomId = pr.RoomId,
                            CategoryPriorityPackageRoomId = pr.CategoryPriorityPackageRoomId,
                            StartDate = pr.StartDate,
                            EndDate = pr.EndDate,
                            Price = pr.Price,
                            UserId = pr.UserId,
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();
            }
        }

        public static async Task<Room> FindRoomByIdAsync(int roomId)
        {
            Room room = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    room = await context.Rooms.SingleOrDefaultAsync(x => x.RoomId == roomId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return room;
        }
        public static async Task<List<RoomDTO>> GetRoomsByStatusAsync(int landlordId, int status)
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.Rooms
                    .Include(r => r.PriorityPackageRooms)
                    .Where(p => p.UserId == landlordId && p.status == status)
                    .Select(p => new RoomDTO
                    {
                        RoomId = p.RoomId,
                        BuildingId = p.BuildingId,
                        UserId = p.UserId,
                        UserName = p.User.UserName,
                        Title = p.Title,
                        Description = p.Description,
                        LocationDetail = p.LocationDetail,
                        Acreage = p.Acreage,
                        Furniture = p.Furniture,
                        NumberOfBathroom = p.NumberOfBathroom,
                        NumberOfBedroom = p.NumberOfBedroom,
                        Garret = p.Garret,
                        Price = p.Price,
                        CategoryRoomId = p.CategoryRoomId,
                        Image = p.Image,
                        Note = p.Note,
                        status = p.status,
                        reputation = p.reputation,
                        Deposit = p.Deposit,
                        Dien = p.Dien,
                        Nuoc = p.Nuoc,
                        Internet = p.Internet,
                        Rac = p.Rac,
                        GuiXe = p.GuiXe,
                        QuanLy = p.QuanLy,
                        ChiPhiKhac = p.ChiPhiKhac,
                        BuildingName = p.Building != null ? p.Building.BuildingName : null,
                        CategoryName = p.CategoryRoom.CategoryName,
                        IsPermission = p.IsPermission,
                        Authorization = p.Authorization,
                        PriorityPackageRooms = p.PriorityPackageRooms.Select(pr => new PriorityPackageRoomDTO
                        {
                            PriorityPackageRoomId = pr.PriorityPackageRoomId,
                            RoomId = pr.RoomId,
                            CategoryPriorityPackageRoomId = pr.CategoryPriorityPackageRoomId,
                            StartDate = pr.StartDate,
                            EndDate = pr.EndDate,
                            Price = pr.Price,
                            UserId = pr.UserId,
                        }).ToList()
                    })
                    .ToListAsync();
            }
        }

        public static async Task<List<UserFeedbackDTO>> GetRoomReviewsAsync(int roomId)
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.UserFeedbacks
                    .AsNoTracking()
                    .Where(uf => uf.RoomId == roomId)
                    .Select(uf => new UserFeedbackDTO
                    {
                        UserFeedbackId = uf.UserFeedbackId,
                        UserId = uf.UserId,
                        Comment = uf.Comment,
                        Star = (int)uf.Star,
                        Image = uf.Image,
                        CreatedDate = uf.CreatedDate
                    })
                    .ToListAsync();
            }
        }

        public static async Task SaveRoomAsync(Room room)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.Rooms.AddAsync(room);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateRoomAsync(Room room)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(room).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteRoomAsync(Room room)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingRoom = await context.Rooms.SingleOrDefaultAsync(c => c.RoomId == room.RoomId);
                    if (existingRoom != null)
                    {
                        context.Rooms.Remove(existingRoom);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<RoomDTO>> SearchRoomsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetRoomsAsync();
            }

            try
            {
                using (var context = new ApplicationDbContext())
                {

                    bool isNumeric = int.TryParse(searchTerm, out int numericValue);

                    var room = await context.Rooms
                        .AsNoTracking()
                        .Where(p => p.Title.ToLower().Contains(searchTerm.ToLower().Trim())
                                || (isNumeric && p.Price > numericValue)
                                || p.LocationDetail.ToLower().Contains(searchTerm.ToLower().Trim())
                                )
                        .Select(p => new RoomDTO
                        {
                            RoomId = p.RoomId,
                            BuildingId = p.BuildingId,
                            UserId = p.UserId,
                            UserName = p.User.UserName,
                            Title = p.Title,
                            Description = p.Description,
                            LocationDetail = p.LocationDetail,
                            Acreage = p.Acreage,
                            Furniture = p.Furniture,
                            NumberOfBathroom = p.NumberOfBathroom,
                            NumberOfBedroom = p.NumberOfBedroom,
                            Garret = p.Garret,
                            Price = p.Price,
                            CategoryRoomId = p.CategoryRoomId,
                            Image = p.Image,
                            Note = p.Note,
                            status = p.status,
                            reputation = p.reputation,
                            Deposit = p.Deposit,
                            Dien = p.Dien,
                            Nuoc = p.Nuoc,
                            Internet = p.Internet,
                            Rac = p.Rac,
                            GuiXe = p.GuiXe,
                            QuanLy = p.QuanLy,
                            ChiPhiKhac = p.ChiPhiKhac,
                            BuildingName = p.Building != null ? p.Building.BuildingName : null,
                            CategoryName = p.CategoryRoom.CategoryName,
                            IsPermission = p.IsPermission,
                            Authorization = p.Authorization,
                        })
                        .ToListAsync();

                    return room;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<RoomDTO>> GetAllRoomsByStatusAsync(int status)
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.Rooms
                     .Include(r => r.PriorityPackageRooms)
                    .Where(p => p.status == 1 && p.IsPermission == 1)
                    .Select(p => new RoomDTO
                    {
                        RoomId = p.RoomId,
                        BuildingId = p.BuildingId,
                        UserId = p.UserId,
                        UserName = p.User.UserName,
                        Title = p.Title,
                        Description = p.Description,
                        LocationDetail = p.LocationDetail,
                        Acreage = p.Acreage,
                        Furniture = p.Furniture,
                        NumberOfBathroom = p.NumberOfBathroom,
                        NumberOfBedroom = p.NumberOfBedroom,
                        Garret = p.Garret,
                        Price = p.Price,
                        CategoryRoomId = p.CategoryRoomId,
                        Image = p.Image,
                        Note = p.Note,
                        status = p.status,
                        reputation = p.reputation,
                        Deposit = p.Deposit,
                        Dien = p.Dien,
                        Nuoc = p.Nuoc,
                        Internet = p.Internet,
                        Rac = p.Rac,
                        GuiXe = p.GuiXe,
                        QuanLy = p.QuanLy,
                        ChiPhiKhac = p.ChiPhiKhac,
                        BuildingName = p.Building != null ? p.Building.BuildingName : null,
                        CategoryName = p.CategoryRoom.CategoryName,
                        IsPermission = p.IsPermission,
                        Authorization = p.Authorization,
                        PriorityPackageRooms = p.PriorityPackageRooms.Select(pr => new PriorityPackageRoomDTO
                        {
                            PriorityPackageRoomId = pr.PriorityPackageRoomId,
                            RoomId = pr.RoomId,
                            CategoryPriorityPackageRoomId = pr.CategoryPriorityPackageRoomId,
                            StartDate = pr.StartDate,
                            EndDate = pr.EndDate,
                            Price = pr.Price,
                            UserId = pr.UserId,
                        }).ToList()
                    })
                    .ToListAsync();
            }
        }

        public static async Task<RoomDTO> GetRoomContractByIdAsync(int roomId)
        {
            using (var context = new ApplicationDbContext())
            {
                var room = await context.Rooms
                    .Where(r => r.RoomId == roomId)
                    .AsNoTracking()
                    .Include(r => r.RentalLists) // Đảm bảo tải RentalLists
                    .ThenInclude(rl => rl.User) // Tải thông tin Renter (User)
                    .FirstOrDefaultAsync();

                if (room == null) return null;

                return new RoomDTO
                {
                    RoomId = room.RoomId,
                    UserId = room.UserId,
                    User = room.User != null ? new UserDTO
                    {
                        UserId = room.User.UserId,
                        Name = room.User.Name,
                        Gmail = room.User.Gmail,
                        Phone = room.User.Phone
                    } : null,
                    Title = room.Title,
                    Description = room.Description,
                    LocationDetail = room.LocationDetail,
                    Acreage = room.Acreage,
                    Furniture = room.Furniture,
                    NumberOfBathroom = room.NumberOfBathroom,
                    NumberOfBedroom = room.NumberOfBedroom,
                    Garret = room.Garret,
                    Price = room.Price,
                    Deposit = room.Deposit,
                    Image = room.Image,
                    Note = room.Note,
                    status = room.status,
                    Dien = room.Dien,
                    Nuoc = room.Nuoc,
                    Internet = room.Internet,
                    Rac = room.Rac,
                    GuiXe = room.GuiXe,
                    QuanLy = room.QuanLy,
                    ChiPhiKhac = room.ChiPhiKhac,
                    IsPermission = room.IsPermission,
                    reputation = room.reputation,
                    Authorization = room.Authorization,
                    RentalLists = room.status == 2 && room.RentalLists != null ? room.RentalLists
                        .Select(rl => new RentalListDTO
                        {
                            RentalId = rl.RentalId,
                            RoomId = rl.RoomId,
                            RenterID = rl.RenterID,
                            RenterName = rl.User != null ? rl.User.Name : "Không có",
                            RenterEmail = rl.User != null ? rl.User.Gmail : "Không có",
                            RenterPhone = rl.User != null ? rl.User.Phone : "Không có",
                            MonthForRent = rl.MonthForRent,
                            RentDate = rl.RentDate,
                            RentalStatus = rl.RentalStatus
                        }).ToList() : null
                };
            }
        }
        public static async Task<List<RoomDTO>> GetRoomRegisterReputationAsync()
        {
            using var context = new ApplicationDbContext();
            var rooms = await context.Rooms
                .Where(r => r.reputation == 0 && context.RoomLicenses
                    .Any(rl => rl.RoomId == r.RoomId && !string.IsNullOrEmpty(rl.BienBanPCCC)))
                .Select(r => new RoomDTO
                {
                    RoomId = r.RoomId,
                    BuildingId = r.BuildingId,
                    UserId = r.UserId,
                    UserName = r.User.UserName,
                    Title = r.Title,
                    Description = r.Description,
                    LocationDetail = r.LocationDetail,
                    Acreage = r.Acreage,
                    Furniture = r.Furniture,
                    NumberOfBathroom = r.NumberOfBathroom,
                    NumberOfBedroom = r.NumberOfBedroom,
                    Garret = r.Garret,
                    Price = r.Price,
                    CategoryRoomId = r.CategoryRoomId,
                    Image = r.Image,
                    Note = r.Note,
                    Dien = r.Dien,
                    Nuoc = r.Nuoc,
                    Internet = r.Internet,
                    Rac = r.Rac,
                    GuiXe = r.GuiXe,
                    QuanLy = r.QuanLy,
                    ChiPhiKhac = r.ChiPhiKhac,
                    BuildingName = r.Building != null ? r.Building.BuildingName : null,
                    CategoryName = r.CategoryRoom.CategoryName,
                    IsPermission = r.IsPermission,
                    status = r.status,
                    Authorization = r.Authorization,
                })
                .ToListAsync();

            return rooms;
        }
        public static async Task LockRoomAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var room = await context.Rooms.FirstOrDefaultAsync(u => u.RoomId == roomId);
                    if (room == null)
                    {
                        throw new KeyNotFoundException($"Room với ID {roomId} không tồn tại.");
                    }

                    room.IsPermission = 0;
                    context.Rooms.Update(room);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi khóa Room: {ex.Message}");
            }
        }

        public static async Task UnLockRoomAsync(int roomId, int userId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    // Tìm phòng
                    var room = await context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
                    if (room == null)
                    {
                        throw new KeyNotFoundException($"Phòng với ID {roomId} không tồn tại.");
                    }

                    // Tìm người dùng để kiểm tra RoleAdmin
                    var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                    if (user == null)
                    {
                        throw new InvalidOperationException("Người dùng không tồn tại.");
                    }
                    bool isAdmin = user.RoleAdmin == 1;

                    // Kiểm tra trạng thái IsPermission
                    if (room.IsPermission == 2 && !isAdmin)
                    {
                        throw new InvalidOperationException("Phòng bị khóa bởi Admin, bạn không thể mở khóa.");
                    }

                    if (room.IsPermission != 0 && room.IsPermission != 2)
                    {
                        throw new InvalidOperationException("Phòng không ở trạng thái khóa, không thể mở khóa.");
                    }

                    room.IsPermission = 1;
                    context.Rooms.Update(room);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi mở khóa phòng: {ex.Message}");
            }
        }
        public static async Task AcceptReputationAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var room = await context.Rooms.FirstOrDefaultAsync(u => u.RoomId == roomId);
                    if (room == null)
                    {
                        throw new KeyNotFoundException($"Room với ID {roomId} không tồn tại.");
                    }

                    room.reputation = 1;
                    context.Rooms.Update(room);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi Accept Reputation: {ex.Message}");
            }
        }
        public static async Task CancelReputationAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var room = await context.Rooms.FirstOrDefaultAsync(u => u.RoomId == roomId);
                    if (room == null)
                    {
                        throw new KeyNotFoundException($"Room với ID {roomId} không tồn tại.");
                    }
                    var roomLicense = await context.RoomLicenses.FirstOrDefaultAsync(rl => rl.RoomId == roomId);
                    if (roomLicense != null)
                    {
                        context.RoomLicenses.Remove(roomLicense);
                        //roomLicense.BienBanPCCC = null;
                        //context.RoomLicenses.Update(roomLicense);
                    }
                    room.reputation = 0;
                    context.Rooms.Update(room);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi Cancel Reputation: {ex.Message}");
            }
        }
        public static async Task<Room?> GetRoomEntityByIdForLandlordAsync(int roomId, int landlordId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Rooms
                        .FirstOrDefaultAsync(r => r.RoomId == roomId && r.UserId == landlordId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy Room entity: {ex.Message}");
            }
        }
        public static async Task<Room?> GetRoomEntityByIdAsync(int roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return await context.Rooms
                        .FirstOrDefaultAsync(r => r.RoomId == roomId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy Room entity: {ex.Message}");
            }
        }
        public static async Task<bool> CheckRoomIsDuplicatedAsync(int userId, string title, string description)
        {
            try
            {
                using var context = new ApplicationDbContext();
                return await context.Rooms
                    .AnyAsync(r => r.UserId == userId
                                && r.Title.ToLower() == title.ToLower()
                                //&& r.LocationDetail.ToLower() == locationDetail.ToLower()
                                && r.Description.ToLower() == description.ToLower()); // Kiểm tra Description
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra phòng trùng lặp: {ex.Message}");
            }
        }


        public static async Task<bool> CheckDescriptionExistsAsync(string description)
        {
            using var context = new ApplicationDbContext();
            return await context.Rooms.AnyAsync(r => r.Description.ToLower() == description.ToLower());
        }
        public static async Task<bool> CheckLocationExistsAsync(string locationDetail)
        {
            try
            {
                using var context = new ApplicationDbContext();
                return await context.Rooms
                    .AnyAsync(r => r.LocationDetail.ToLower() == locationDetail.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra LocationDetail trùng: {ex.Message}");
            }
        }
        public static async Task UpdateAuthorizationAsync(int roomId, int authorization)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var room = await context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
                    if (room == null)
                    {
                        throw new KeyNotFoundException($"Room với ID {roomId} không tồn tại.");
                    }

                    room.Authorization = authorization;
                    context.Rooms.Update(room);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật Authorization: {ex.Message}");
            }
        }
        public static async Task<List<RoomDTO>> GetRoomAuthorizationAsync(int? userId)
        {
            using var context = new ApplicationDbContext();

            var query = context.Rooms
                .Include(p => p.User)
                .Include(p => p.Building)
                .Include(p => p.CategoryRoom)
                .Where(p => p.Authorization == 1)
                .AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId);
            }

            return await query
                .AsNoTracking()
                .Select(p => new RoomDTO
                {
                    RoomId = p.RoomId,
                    BuildingId = p.BuildingId,
                    UserId = p.UserId,
                    UserName = p.User.UserName,
                    Title = p.Title,
                    Description = p.Description,
                    LocationDetail = p.LocationDetail,
                    Acreage = p.Acreage,
                    Furniture = p.Furniture,
                    NumberOfBathroom = p.NumberOfBathroom,
                    NumberOfBedroom = p.NumberOfBedroom,
                    Garret = p.Garret,
                    Price = p.Price,
                    CategoryRoomId = p.CategoryRoomId,
                    Image = p.Image,
                    Note = p.Note,
                    status = p.status,
                    reputation = p.reputation,
                    Deposit = p.Deposit,
                    Dien = p.Dien,
                    Nuoc = p.Nuoc,
                    Internet = p.Internet,
                    Rac = p.Rac,
                    GuiXe = p.GuiXe,
                    QuanLy = p.QuanLy,
                    ChiPhiKhac = p.ChiPhiKhac,
                    BuildingName = p.Building != null ? p.Building.BuildingName : null,
                    CategoryName = p.CategoryRoom.CategoryName,
                    IsPermission = p.IsPermission,
                    Authorization = p.Authorization
                })
                .ToListAsync();
        }
        public static async Task<Dictionary<int, List<int>>> GetRoomIdsGroupedByBuildingAsync(int userId)
        {
            using var context = new ApplicationDbContext();

            var roomGroups = await context.Rooms
                .Where(r => r.UserId == userId && r.BuildingId != null)
                .GroupBy(r => r.BuildingId.Value)
                .Select(g => new
                {
                    BuildingId = g.Key,
                    RoomIds = g.Select(r => r.RoomId).ToList()
                })
                .ToListAsync();

            return roomGroups.ToDictionary(g => g.BuildingId, g => g.RoomIds);
        }
        public static async Task<List<RoomDTO>> SearchRoomsByTermAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetRoomsAsync();
            }

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    searchTerm = searchTerm.Trim().ToLower();

                    var rooms = await context.Rooms
                        .AsNoTracking()
                        .Where(r =>
                            r.Title.ToLower().Contains(searchTerm) ||
                            r.Description.ToLower().Contains(searchTerm) ||
                            r.LocationDetail.ToLower().Contains(searchTerm))
                        .Select(r => new RoomDTO
                        {
                            RoomId = r.RoomId,
                            BuildingId = r.BuildingId,
                            UserId = r.UserId,
                            UserName = r.User.UserName,
                            Title = r.Title,
                            Description = r.Description,
                            LocationDetail = r.LocationDetail,
                            Acreage = r.Acreage,
                            Furniture = r.Furniture,
                            NumberOfBathroom = r.NumberOfBathroom,
                            NumberOfBedroom = r.NumberOfBedroom,
                            Garret = r.Garret,
                            Price = r.Price,
                            CategoryRoomId = r.CategoryRoomId,
                            CategoryName = r.CategoryRoom.CategoryName,
                            Image = r.Image,
                            Note = r.Note,
                            IsPermission = r.IsPermission
                        })
                        .ToListAsync();

                    // Nếu tìm không ra → fallback bằng cách tách từ ra và tìm theo từng từ
                    if (!rooms.Any())
                    {
                        var keywords = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        foreach (var keyword in keywords)
                        {
                            var partialRooms = await context.Rooms
                                .AsNoTracking()
                                .Where(r =>
                                    r.Title.ToLower().Contains(keyword) ||
                                    r.Description.ToLower().Contains(keyword) ||
                                    r.LocationDetail.ToLower().Contains(keyword))
                                .Select(r => new RoomDTO
                                {
                                    RoomId = r.RoomId,
                                    BuildingId = r.BuildingId,
                                    UserId = r.UserId,
                                    UserName = r.User.UserName,
                                    Title = r.Title,
                                    Description = r.Description,
                                    LocationDetail = r.LocationDetail,
                                    Acreage = r.Acreage,
                                    Furniture = r.Furniture,
                                    NumberOfBathroom = r.NumberOfBathroom,
                                    NumberOfBedroom = r.NumberOfBedroom,
                                    Garret = r.Garret,
                                    Price = r.Price,
                                    CategoryRoomId = r.CategoryRoomId,
                                    CategoryName = r.CategoryRoom.CategoryName,
                                    Image = r.Image,
                                    Note = r.Note,
                                    IsPermission = r.IsPermission
                                })
                                .ToListAsync();

                            // Ghép kết quả không trùng nhau
                            rooms.AddRange(partialRooms.Where(x => !rooms.Any(r => r.RoomId == x.RoomId)));
                        }
                    }

                    return rooms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm phòng: {ex.Message}");
            }
        }

    }
}