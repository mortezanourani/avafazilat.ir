using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Message1
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public Guid DepartmentId { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public DateTime Submitted { get; set; }

    public bool IsRead { get; set; }

    public virtual Department Department { get; set; }
}
