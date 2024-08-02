using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Invoice
{
    public Guid Id { get; set; }

    public string Issued { get; set; } = null!;

    public string DueDate { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public string SubTotal { get; set; } = null!;

    public string Discount { get; set; } = null!;

    public virtual AspNetUser Customer { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
