using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Service
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int Status { get; set; }

    public decimal? Cost { get; set; }

    public int? Duration { get; set; }

    public int? Rank { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
