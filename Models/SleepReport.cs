using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class SleepReport
{
    public string UserId { get; set; }

    public string Day { get; set; }

    public string BedTime { get; set; }

    public string WakeUp { get; set; }

    public string InBed { get; set; }

    public byte Point { get; set; }

    public virtual AspNetUser User { get; set; }
}
