using Fazilat.Models;
using Microsoft.AspNetCore.Http;

namespace Fazilat.Areas.Dashboard.Models
{
    public class AddMediaViewModel : Media
    {
        public virtual IFormFile File { get; set; }
    }
}
