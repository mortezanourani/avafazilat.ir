using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Slide
{
    public Guid Id { get; set; }

    public Guid ImageId { get; set; }

    public string? Title { get; set; }

    public string? Caption { get; set; }

    public string? Link { get; set; }

    public string Created { get; set; } = null!;

    public bool IsVisible { get; set; }

    public virtual Media Image { get; set; } = null!;
}
