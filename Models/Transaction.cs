using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid InvoiceId { get; set; }

    public string Amount { get; set; }

    public string TrackingCode { get; set; }

    public string ReferenceNumber { get; set; }

    public string Paid { get; set; }

    public string Status { get; set; }

    public virtual Invoice Invoice { get; set; }

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
