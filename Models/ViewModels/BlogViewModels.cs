using System.Collections;
using System.Collections.Generic;

namespace Fazilat.Models.ViewModels
{
    public class BlogViewModels
    {
        public Post LastPost { get; set; }
        public ICollection<Post> PostsList { get; set; }
        public int Offset { get; set; }
        public bool hasPrevious { get; set; }
        public bool hasNext { get; set; }
        public ICollection<AspNetUser> AuthorsList { get; set; }
    }
}
