using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Course
{
    public Guid Id { get; set; }

    public Guid GradeId { get; set; }

    public string Title { get; set; } = null!;

    public virtual Grade Grade { get; set; } = null!;

    public virtual ICollection<ScheduleTask> ScheduleTasks { get; set; } = new List<ScheduleTask>();
}
