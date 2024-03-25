using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class TicketInstruction
{
    public string Id { get; set; }

    public bool IsActive { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}
