using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Schedule
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public string CounsellorId { get; set; } = null!;

    public Guid Name { get; set; }

    public string Start { get; set; } = null!;

    public string Finish { get; set; } = null!;

    public string? Instruction { get; set; }

    public virtual AspNetUser Counsellor { get; set; } = null!;

    public virtual ICollection<ScheduleTask> ScheduleTasks { get; set; } = new List<ScheduleTask>();

    public virtual AspNetUser User { get; set; } = null!;
}
