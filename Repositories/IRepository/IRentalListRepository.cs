using DTO;
using DUVAS;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRentalListRepository
{
    Task<RentalList> GetRentalListByIdAsync(int rentalId);
    Task<RentalList> GetRentalListByRoomIdAsync(int roomId);
    Task<RentalList> GetRentalListByRoomIdAndRenterIdAsync(int roomId, int renterId);
    Task<List<RentalList>> GetPendingRentalListsByRoomIdAsync(int roomId);
    Task<List<RentalList>> GetConfirmedRentalListsByRoomIdAsync(int roomId);
    Task<List<RentalListDTO>> GetRentalListsAsync();
    Task<List<RentalListDTO>> GetRentalsByUserIdAsync(int id);
    Task SaveRentalListAsync(RentalList rental);
    Task UpdateRentalListAsync(RentalList rental);
    Task UpdateRentalListContractAsync(int rentalId, int contractId);
    Task UpdateRentalListStatusAsync(int rentalId, int status);
    Task DeleteRentalListAsync(RentalList rental);
}