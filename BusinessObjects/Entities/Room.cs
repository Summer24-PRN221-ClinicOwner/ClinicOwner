using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Room
{
    public int Id { get; set; }

    public string? RoomNumber { get; set; }

    public int? ClinicId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Clinic? Clinic { get; set; }

    public virtual ICollection<RoomAvailability> RoomAvailabilities { get; set; } = new List<RoomAvailability>();
}
