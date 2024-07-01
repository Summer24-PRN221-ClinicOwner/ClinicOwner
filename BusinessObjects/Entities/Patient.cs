using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Patient
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User? IdNavigation { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
