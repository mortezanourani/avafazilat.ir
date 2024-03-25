using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Meeting
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string Major { get; set; }

    public string Type { get; set; }

    public byte[] Payment { get; set; }

    public string TicketId { get; set; }

    public bool Confirmed { get; set; }

    public virtual Ticket Ticket { get; set; }
}
