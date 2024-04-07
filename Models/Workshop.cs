using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Workshop
{
    public Guid Id { get; set; }

    public int Grade { get; set; }

    public string Title { get; set; }

    public string Cost { get; set; }

    public int Capacity { get; set; }

    public string StartDate { get; set; }

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
