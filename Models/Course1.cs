using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Course1
{
    public Guid Id { get; set; }

    public Guid GradeId { get; set; }

    public string Title { get; set; }

    public virtual Grade Grade { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
