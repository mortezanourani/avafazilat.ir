using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public string PostalCode { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public string? District { get; set; }

    public Guid CityId { get; set; }

    public string? Phone { get; set; }

    public virtual City City { get; set; } = null!;
}
