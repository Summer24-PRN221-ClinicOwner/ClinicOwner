using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public partial class Room
    {
        public int Id { get; set; }

        public string? RoomNumber { get; set; }

        public int? ClinicId { get; set; }

        public virtual Clinic? Clinic { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public virtual ICollection<RoomAvailability> RoomAvailabilities { get; set; } = new HashSet<RoomAvailability>();
    } 
}
