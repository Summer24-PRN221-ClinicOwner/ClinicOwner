﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public bool IsRead { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
