using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Province
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
