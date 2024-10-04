using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Post
{
    public Guid Id { get; set; }

    public string AuthorId { get; set; }

    public Guid HeaderId { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public bool IsVisible { get; set; }

    public DateTime Published { get; set; }

    public virtual AspNetUser Author { get; set; }

    public virtual Media Header { get; set; }
}
