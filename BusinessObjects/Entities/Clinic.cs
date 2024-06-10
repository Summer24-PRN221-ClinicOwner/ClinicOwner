using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Clinic
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public int OwnerId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Dentist> Dentists { get; set; } = new List<Dentist>();

    public virtual ClinicOwner Owner { get; set; } = null!;
}
