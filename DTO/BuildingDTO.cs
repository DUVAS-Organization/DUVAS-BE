using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BuildingDTO
    {
        [Key]
        public int BuildingId { get; set; }

        public string BuildingName { get; set; }
        public string Location { get; set; }
        public bool Verify { get; set; }
        public string CategoryName { get; set; }
    }
}
