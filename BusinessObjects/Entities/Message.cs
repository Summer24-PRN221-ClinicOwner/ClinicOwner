using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Message
{
    public int MessageId { get; set; }

    public int? DentistId { get; set; }

    public int? PatientId { get; set; }

    public string? Content { get; set; }

    public DateTime? DateSend { get; set; }

    public DateTime? DateSeen { get; set; }

    public virtual Dentist? Dentist { get; set; }

    public virtual Patient? Patient { get; set; }
}
