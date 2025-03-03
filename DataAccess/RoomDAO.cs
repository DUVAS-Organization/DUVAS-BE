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
                            BuildingName = p.Building.BuildingName,
                            CategoryName = p.CategoryRoom.CategoryName,    

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            


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

        public static async Task<List<RoomDTO>> GetRoomsByLandlordAsync(int userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.Rooms
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
                        IsPermission = p.IsPermission
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
                        IsPermission = (bool)p.IsPermission
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
                            BuildingName = p.Building.BuildingName,
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
                            CategoryName = p.CategoryRoom.CategoryName,

                            Image = p.Image,
                            Note = p.Note,
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
    }
}
