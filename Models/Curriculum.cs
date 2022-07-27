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
        [Required(ErrorMessage = "این مورد الزامی است.")]
        [Display(Name = "عنوان برنامه")]
        [RegularExpression("^[آ-یای ]+$", ErrorMessage = "لطفا به صورت فارسی وارد گردد.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "این مورد الزامی است.")]
        [Display(Name = "تاریخ آغاز برنامه")]
        public DateTime StartDate { get; set; }
        [Display(Name = "توضیحات برنامه هفتگی")]
        [RegularExpression("^[آ-یای ]+$", ErrorMessage = "لطفا به صورت فارسی وارد گردد.")]
        public string Description { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
