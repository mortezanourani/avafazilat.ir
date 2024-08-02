using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Grade
{
    public Guid Id { get; set; }

    public Guid FieldId { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string PersianName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Field Field { get; set; } = null!;

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
