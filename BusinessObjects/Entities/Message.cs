using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Message
{
    public int Id { get; set; }

    public int? DentistId { get; set; }

    public int? PatientId { get; set; }

    public string? Content { get; set; }

    public DateTime? DateSend { get; set; }

    public DateTime? DateSeen { get; set; }

    public string Sender { get; set; } = null!;

    public virtual Dentist? Dentist { get; set; }

    public virtual Patient? Patient { get; set; }
}
