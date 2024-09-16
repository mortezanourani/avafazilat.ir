using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Schedule
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string CounsellorId { get; set; }

    public Guid Name { get; set; }

    public string Start { get; set; }

    public string Finish { get; set; }

    public string Instruction { get; set; }

    public virtual AspNetUser Counsellor { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual AspNetUser User { get; set; }
}
