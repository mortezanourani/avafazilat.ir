using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Models
{
    public class Curriculum
    {
        public string Id { get; set; }
        [Display(Name = "عنوان برنامه")]
        public string Title { get; set; }
        [Display(Name = "تاریخ آغاز برنامه")]
        public DateTime StartDate { get; set; }
        [Display(Name = "توضیحات برنامه هفتگی")]
        public string Description { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
