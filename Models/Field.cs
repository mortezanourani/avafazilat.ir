using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Field
{
    public Guid Id { get; set; }

    public Guid DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string PersianName { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
