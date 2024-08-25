using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string PersianName { get; set; }

    public virtual ICollection<Field> Fields { get; set; } = new List<Field>();

    public virtual ICollection<Message1> Message1s { get; set; } = new List<Message1>();

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
