using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Curriculum
{
    public string Id { get; set; }

    public string Title { get; set; }

    public DateOnly StartDate { get; set; }

    public string Description { get; set; }

    public string UserId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual User User { get; set; }
}
