using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Media
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;

    public string Extension { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public string Uploaded { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Slide> Slides { get; set; } = new List<Slide>();
}
