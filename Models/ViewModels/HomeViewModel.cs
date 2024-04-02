using System.Collections.Generic;

namespace Fazilat.Models.ViewModels;

public class HomeViewModel
{
    public ICollection<Slide> Slides { get; set; }

    public ICollection<BlogPost> Posts { get; set; }
}
