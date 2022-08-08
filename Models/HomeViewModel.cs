using System.Collections.Generic;

namespace Fazilat.Models
{
    public class HomeViewModel
    {
        public ICollection<Slide> Slides { get; set; }
        public ICollection<BlogPost> News { get; set; }
    }
}
