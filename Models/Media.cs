using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Media
{
    public Guid Id { get; set; }

    public string FileName { get; set; }

    public string Extension { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime Uploaded { get; set; }

    public virtual ICollection<Banner> Banners { get; set; } = new List<Banner>();

    public virtual Category Category { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Slide1> Slide1s { get; set; } = new List<Slide1>();
}
