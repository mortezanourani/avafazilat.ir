using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Message
{
    public string Id { get; set; }

    public string SenderId { get; set; }

    public string ReceiverId { get; set; }

    public string Text { get; set; }

    public DateTime Created { get; set; }

    public virtual User Receiver { get; set; }

    public virtual User Sender { get; set; }
}
