using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class ActivityReport
{
    public string UserId { get; set; }

    public string Day { get; set; }

    public Guid TaskId { get; set; }

    public string Duration { get; set; }

    public virtual Task Task { get; set; }

    public virtual AspNetUser User { get; set; }
}
