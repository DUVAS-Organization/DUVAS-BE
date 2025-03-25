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
    namespace Repositories
    {
        public class InsiderTradingRepository : IInsiderTradingRepository
        {
            public async Task DeleteInsiderTradingAsync(int id) => await InsiderTradingDAO.DeleteInsiderTradingAsync(id);
            public async Task<InsiderTradingDTO> GetInsiderTradingByIdAsync(int id) => await InsiderTradingDAO.FindInsiderTradingByIdAsync(id);
            public async Task<List<InsiderTradingDTO>> GetInsiderTradingsAsync() => await InsiderTradingDAO.GetInsiderTradingsAsync();

            public async Task<int> NewInsiderTradingAsync(InsiderTradingDTO b, string type)
            {
                await InsiderTradingDAO.SaveInsiderTradingAsync(b, type);
                return b.InsiderTradingId;
            }

            public async Task SaveInsiderTradingAsync(InsiderTradingDTO b, string type) => await InsiderTradingDAO.SaveInsiderTradingAsync(b, type);
            public async Task UpdateInsiderTradingAsync(InsiderTradingDTO b) => await InsiderTradingDAO.UpdateInsiderTradingAsync(b);
        }
    }
}