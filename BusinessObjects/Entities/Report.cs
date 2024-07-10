namespace BusinessObjects.Entities;

public partial class Report
{
    public int Id { get; set; }

    public int AppointmentId { get; set; }

    public string? Name { get; set; }
    public string? Data { get; set; }
    public DateTime? GeneratedDate { get; set; }

    public virtual Appointment? Appointment { get; set; } = null!;
}
