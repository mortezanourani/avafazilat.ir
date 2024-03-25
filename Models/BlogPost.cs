using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class BlogPost
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public DateTime Date { get; set; }

    public string Title { get; set; }

    public string Image { get; set; }

    public string Content { get; set; }

    public bool IsVisible { get; set; }
}
