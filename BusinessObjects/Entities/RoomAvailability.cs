using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class RoomAvailability
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public string? AvailableSlots { get; set; }

    public DateTime Day { get; set; }

    public virtual Room Room { get; set; } = null!;
}
