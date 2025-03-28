using DTO;
using DUVAS;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IInsiderTradingRepository
    {
        Task SaveInsiderTradingAsync(InsiderTradingDTO b, string type);
        Task<int> NewInsiderTradingAsync(InsiderTradingDTO b, string type);
        Task<InsiderTradingDTO> GetInsiderTradingByIdAsync(int id);
        Task DeleteInsiderTradingAsync(int id);
        Task UpdateInsiderTradingAsync(InsiderTradingDTO b);
        Task<List<InsiderTradingDTO>> GetInsiderTradingsAsync();
        Task UpdateInsiderTradingStatusAsync(int id, int status);
    }
}