using DTO;
using DUVAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IBuildingRepository
    {
        Task SaveBuildingAsync(Building b);
        Task<Building> GetBuildingByIdAsync(int id);
        Task DeleteBuildingAsync(Building b);
        Task UpdateBuildingAsync(Building b);
        Task<List<BuildingDTO>> GetBuildingsAsync();
        Task<List<BuildingDTO>> SearchBuildingsAsync(string searchTerm);

    }
}
