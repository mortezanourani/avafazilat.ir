using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Receipt
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }

    public virtual Transaction Transaction { get; set; }
}
