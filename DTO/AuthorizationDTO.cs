using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{

    public class UpdateRoomsAuthorizationRequest
    {
        public List<int> RoomIds { get; set; }
        public int Authorization { get; set; }
    }

    public class UpdateContractsStatusRequest
    {
        public List<int> ContractIds { get; set; }
        public int Status { get; set; }
    }

}
