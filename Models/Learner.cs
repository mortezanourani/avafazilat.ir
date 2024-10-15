using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Learner
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string ParentPhone { get; set; }

    public string City { get; set; }

    public string District { get; set; }

    public string School { get; set; }

    public string TrackingCode { get; set; }

    public DateTime Registered { get; set; }

    public virtual ICollection<Workshop> Workshops { get; set; } = new List<Workshop>();
}
