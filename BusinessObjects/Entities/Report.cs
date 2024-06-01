using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Report
{
    public int ReportId { get; set; }

    public string? ReportName { get; set; }

    public string? ReportData { get; set; }

    public DateTime? GeneratedDate { get; set; }
}
