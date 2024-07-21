namespace BusinessObjects.Entities;

public partial class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string PaymentStatus { get; set; } = null!;
    public DateTime PaymentDate { get; set; }
    public string? TransactionId { get; set; }
    public string? TransactionNo { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
