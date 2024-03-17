using Fazilat.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Panel.Models
{
    public class AddMediaViewModel : Media
    {
        [Display(Name = "فایل")]
        public virtual IFormFile File { get; set; }
    }
}
