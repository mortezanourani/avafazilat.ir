using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class FinancialRecord
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string TrackingCode { get; set; }

    public string Description { get; set; }

    public string PaymentReceipt { get; set; }

    public bool IsApproved { get; set; }

    public virtual User User { get; set; }
}
