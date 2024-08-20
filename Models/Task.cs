using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public Guid ScheduleId { get; set; }

    public Guid CourseId { get; set; }

    public string Topics { get; set; }

    public string Instruction { get; set; }

    public bool IsDone { get; set; }

    public string Report { get; set; }

    public virtual ICollection<ActivityReport> ActivityReports { get; set; } = new List<ActivityReport>();

    public virtual Course1 Course { get; set; }

    public virtual Schedule Schedule { get; set; }
}
