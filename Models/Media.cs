using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Media
{
    public Guid Id { get; set; }

    public string FileName { get; set; }

    public string Extension { get; set; }

    public Guid CategoryId { get; set; }

    public string Uploaded { get; set; }

    public virtual Category Category { get; set; }
}
