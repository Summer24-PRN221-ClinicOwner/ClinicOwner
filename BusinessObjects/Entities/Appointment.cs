using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Appointment
{
    public int Id { get; set; }

    public int? PatientId { get; set; }

    public int? DentistId { get; set; }

    public int? StartSlot { get; set; }

    public int? ServiceId { get; set; }

    public int? RoomId { get; set; }

    public int? Status { get; set; }

    public int? EndSlot { get; set; }

    public DateTime AppointDate { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime ModifyDate { get; set; }

    public virtual Dentist? Dentist { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Service? Service { get; set; }
}
