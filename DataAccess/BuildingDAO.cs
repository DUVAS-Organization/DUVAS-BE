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
    public class BuildingDAO
    {
        private readonly ApplicationDbContext _context;

        public BuildingDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public static async Task<List<BuildingDTO>> GetBuildingsAsync()
        {

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var buildings = await context.Buildings
                        .AsNoTracking()
                        .Select(p => new BuildingDTO
                        {
                            BuildingId = p.BuildingId,
                            BuildingName = p.BuildingName,
                            Location = p.Location,

                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,                            

                        })
                        .ToListAsync();


                    return buildings;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task<Building> FindBuildingByIdAsync(int buildingId)
        {
            Building building = null;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    building = await context.Buildings.SingleOrDefaultAsync(x => x.BuildingId == buildingId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return building;
        }

        public static async Task SaveBuildingAsync(Building building)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    await context.Buildings.AddAsync(building);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateBuildingAsync(Building building)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(building).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteBuildingAsync(Building building)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingBuilding = await context.Buildings.SingleOrDefaultAsync(c => c.BuildingId == building.BuildingId);
                    if (existingBuilding != null)
                    {
                        context.Buildings.Remove(existingBuilding);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<BuildingDTO>> SearchBuildingsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetBuildingsAsync();
            }

            try
            {
                using (var context = new ApplicationDbContext())
                {

                    bool isNumeric = int.TryParse(searchTerm, out int numericValue);

                    var building = await context.Buildings
                        .AsNoTracking()
                        .Where(p => p.BuildingName.ToLower().Contains(searchTerm.ToLower().Trim())
                                //|| (isNumeric && p.Price > numericValue)
                                )
                        .Select(p => new BuildingDTO
                        {
                            BuildingId = p.BuildingId,
                            BuildingName = p.BuildingName,
                            Location = p.Location,
                            //CategoryName = p.Category.CategoryName,
                            //CategoryId = p.CategoryId,
                            //Price = p.Price,
                        })
                        .ToListAsync();

                    return building;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
