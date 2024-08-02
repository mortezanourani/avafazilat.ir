using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class SleepReport
{
    public string UserId { get; set; } = null!;

    public string Day { get; set; } = null!;

    public string BedTime { get; set; } = null!;

    public string WakeUp { get; set; } = null!;

    public string InBed { get; set; } = null!;

    public byte Point { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
