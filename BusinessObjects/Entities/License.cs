using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class License
{
    public int Id { get; set; }

    public string? LicenceType { get; set; }

    public string? LicenseNumber { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public int? Rank { get; set; }

    public virtual ICollection<Dentist> Dentists { get; set; } = new List<Dentist>();
}
