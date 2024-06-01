using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class ClinicOwner
{
    public int OwnerId { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Clinic> Clinics { get; set; } = new List<Clinic>();

    public virtual User? User { get; set; }
}
