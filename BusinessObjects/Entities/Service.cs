using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? Description { get; set; }

    public decimal? Cost { get; set; }

    public int? Duration { get; set; }

    public int? Rank { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
