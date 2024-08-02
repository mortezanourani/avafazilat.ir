using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string PersianName { get; set; } = null!;

    public virtual ICollection<Field> Fields { get; set; } = new List<Field>();

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
