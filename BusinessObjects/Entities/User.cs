using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    // Navigation properties
    public virtual ClinicOwner? ClinicOwner { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Dentist? Dentist { get; set; }
}
