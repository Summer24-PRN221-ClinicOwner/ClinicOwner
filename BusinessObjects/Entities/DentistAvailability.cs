using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class DentistAvailability
{
    public int Id { get; set; }

    public int? DentistId { get; set; }

    public string? AvailableSlots { get; set; }

    public DateTime? Day { get; set; }

    public virtual Dentist? Dentist { get; set; }
}
