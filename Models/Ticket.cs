using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Ticket
{
    public string Id { get; set; }

    public string Day { get; set; }

    public int Hour { get; set; }

    public int Minute { get; set; }

    public bool Reserved { get; set; }

    public bool Taken { get; set; }

    public virtual Meeting Meeting { get; set; }
}
