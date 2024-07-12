using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class ClinicOwner
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Clinic> Clinics { get; set; } = new List<Clinic>();

    public virtual User IdNavigation { get; set; } = null!;
}
