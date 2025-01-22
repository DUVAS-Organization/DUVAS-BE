using System.ComponentModel.DataAnnotations;

namespace DUVAS
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; }

        public string BuildingName { get; set; }
        public string Location { get; set; }
        public bool Verify { get; set; }

        public virtual ICollection<Room>? Rooms { get; set; }
    }
}
