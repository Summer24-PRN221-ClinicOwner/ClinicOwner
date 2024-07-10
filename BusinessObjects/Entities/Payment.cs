using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int AppointmentId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public string? TransactionId { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
