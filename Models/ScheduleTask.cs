using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class ScheduleTask
{
    public Guid Id { get; set; }

    public Guid ScheduleId { get; set; }

    public Guid CourseId { get; set; }

    public string Topics { get; set; } = null!;

    public string Instruction { get; set; } = null!;

    public bool IsDone { get; set; }

    public string Report { get; set; } = null!;

    public virtual ICollection<ActivityReport> ActivityReports { get; set; } = new List<ActivityReport>();

    public virtual Course Course { get; set; } = null!;

    public virtual Schedule Schedule { get; set; } = null!;
}
