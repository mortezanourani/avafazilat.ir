using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Fazilat.Models;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class SlideModel : Slide
    {
        public virtual IFormFile ImageFile { get; set; }
        public ICollection<Slide> Slides { get; set; }
    }
}
