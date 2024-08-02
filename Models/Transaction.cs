using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid InvoiceId { get; set; }

    public string Amount { get; set; } = null!;

    public string? TrackingCode { get; set; }

    public string? ReferenceNumber { get; set; }

    public string Paid { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
