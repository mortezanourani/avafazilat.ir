using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Models
{
    public class Course
    {
        public string Id { get; set; }
        [Display(Name = "عنوان درس")]
        public string Title { get; set; }
        [Display(Name = "سرفصل ها")]
        public string Topics { get; set; }
        public bool Accomplished { get; set; }
        [Display(Name = "توضیح چگونگی اجرا")]
        public string Descritpion { get; set; }
        public string CurriculumId { get; set; }
        public virtual Curriculum Curriculum { get; set; }
    }
}
