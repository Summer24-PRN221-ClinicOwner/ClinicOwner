using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public partial class RoomAvailability
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public string? AvailableSlots { get; set; }

        public DateTime? Day { get; set; }

        public virtual Room Room { get; set; }
    }
}
