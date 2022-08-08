using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

#nullable disable
namespace Fazilat.Models
{
    public class BlogPost
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "عنوان خبر")]
        public string Title { get; set; }
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        [Display(Name = "متن خبر")]
        public string Content { get; set; }
        [Display(Name = "خبر نمایش داده شود؟")]
        public bool isVisible { get; set; }

        public virtual IFormFile ImageFile { get; set; }
    }
}
