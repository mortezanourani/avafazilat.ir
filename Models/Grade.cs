using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Grade
{
    public Guid Id { get; set; }

    public Guid FieldId { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string PersianName { get; set; }

    public virtual ICollection<Course1> Course1s { get; set; } = new List<Course1>();

    public virtual Field Field { get; set; }

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
