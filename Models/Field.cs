using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Field
{
    public Guid Id { get; set; }

    public Guid DepartmentId { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string PersianName { get; set; }

    public virtual Department Department { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
