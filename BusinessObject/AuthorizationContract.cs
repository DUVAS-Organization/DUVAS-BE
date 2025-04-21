using DUVAS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AuthorizationContract
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; } // Số hợp đồng
        //public DateTime Date { get; set; } // Ngày lập hợp đồng
        public int PartyAId { get; set; } // ID của Bên A
        public User? PartyA { get; set; }
        public int PartyBId { get; set; } // ID của Bên B
        public User? PartyB { get; set; }
        public string PdfUrl { get; set; } // URL của file PDF trên Cloudinary
        public string RoomList {  get; set; }
        public int CreatedById { get; set; } // ID của người tạo
        public DateTime CreatedAt { get; set; } // Thời gian tạo
        public int status { get; set; }
        //0: Bị hủy
        //1: Đang hoạt động
        //2: Đang chờ admin duyệt
        //3: Admin đã duyệt
        //4: Hết hạn
    }
}