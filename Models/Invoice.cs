using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Invoice
{
    public Guid Id { get; set; }

    public string Issued { get; set; }

    public string DueDate { get; set; }

    public string CustomerId { get; set; }

    public string SubTotal { get; set; }

    public string Discount { get; set; }

    public virtual AspNetUser Customer { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
