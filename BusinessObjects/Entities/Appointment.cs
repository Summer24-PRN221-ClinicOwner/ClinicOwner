using BusinessObjects.Entities;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public int? DentistId { get; set; }

    public int? StartSlot { get; set; }

    public int? ServiceId { get; set; }

    public int? Status { get; set; }

    public int? EndSlot { get; set; }

    public int ClinicId { get; set; }

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual Dentist? Dentist { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Service? Service { get; set; }
}
