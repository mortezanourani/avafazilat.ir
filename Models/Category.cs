using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string PersianName { get; set; }

    public virtual ICollection<Media> Media { get; set; } = new List<Media>();
}
