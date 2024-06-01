using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public virtual ClinicOwner Id1 { get; set; } = null!;

    public virtual Patient Id2 { get; set; } = null!;

    public virtual Dentist IdNavigation { get; set; } = null!;
}
