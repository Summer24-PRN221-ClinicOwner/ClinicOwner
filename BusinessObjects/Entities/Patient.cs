using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Patient
{
    public int PatientId { get; set; }

    public string? FullName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User? User { get; set; }
}
