using DataAccess;
using DTO;
using DUVAS;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        public async Task DeleteBuildingAsync(Building b) => await BuildingDAO.DeleteBuildingAsync(b);
        public async Task<Building> GetBuildingByIdAsync(int id) => await BuildingDAO.FindBuildingByIdAsync(id);
        public async Task<List<BuildingDTO>> GetBuildingsAsync() => await BuildingDAO.GetBuildingsAsync();
        public async Task SaveBuildingAsync(Building b) => await BuildingDAO.SaveBuildingAsync(b);
        public async Task UpdateBuildingAsync(Building b) => await BuildingDAO.UpdateBuildingAsync(b);
        public async Task<List<BuildingDTO>> SearchBuildingsAsync(string searchTerm) => await BuildingDAO.SearchBuildingsAsync(searchTerm);

    }
}
