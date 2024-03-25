using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class City
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid ProvinceId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Province Province { get; set; }
}
