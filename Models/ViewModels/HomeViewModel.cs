using System.Collections.Generic;

namespace Fazilat.Models.ViewModels;

public class HomeViewModel
{
    public ICollection<Slide1> Slider { get; set; }
    public ICollection<Banner> Banners { get; set; }
    public ICollection<Post> Blog { get; set; }
}
