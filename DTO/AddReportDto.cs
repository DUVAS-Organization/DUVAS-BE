using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AddReportDto
    {
        public int RoomId { get; set; }
        public required string ReportContent { get; set; }
        public required string Image { get; set; }
    }
}
