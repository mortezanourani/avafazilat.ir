using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Banner
{
    public Guid Id { get; set; }

    public Guid ImageId { get; set; }

    public string Link { get; set; }

    public int Position { get; set; }

    public bool IsActive { get; set; }

    public virtual Media Image { get; set; }
}
