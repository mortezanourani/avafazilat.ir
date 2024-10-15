using Fazilat.Models;
using System.Collections.Generic;

namespace Fazilat.Areas.Dashboard.Models
{
    public class AlbumViewMode
    {
        public ICollection<Category> Categories { get; set; }

        public Category Category { get; set; }
    }
}
