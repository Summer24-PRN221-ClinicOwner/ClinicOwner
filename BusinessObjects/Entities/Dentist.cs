using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Dentist
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int ClinicId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual ICollection<DentistAvailability> DentistAvailabilities { get; set; } = new List<DentistAvailability>();

    public virtual ICollection<License> Licenses { get; set; } = new List<License>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
