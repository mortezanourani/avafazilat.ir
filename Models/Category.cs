using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string PersianName { get; set; } = null!;

    public virtual ICollection<Media> Media { get; set; } = new List<Media>();
}
