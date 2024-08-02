using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class ActivityReport
{
    public string UserId { get; set; } = null!;

    public string Day { get; set; } = null!;

    public Guid TaskId { get; set; }

    public string Duration { get; set; } = null!;

    public virtual ScheduleTask Task { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
