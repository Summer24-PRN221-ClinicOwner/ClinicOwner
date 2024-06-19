using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Role { get; set; }

    public int Status { get; set; }

    public virtual ClinicOwner? ClinicOwner { get; set; }

    public virtual Dentist? Dentist { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Patient? Patient { get; set; }
}
