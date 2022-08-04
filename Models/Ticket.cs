using System;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models
{
    public class Ticket
    {
        public string Id { get; set; }

        [Display(Name = "نوبت مشاوره انتخاب رشته")]
        [Required(ErrorMessage = "وارد کردن این مورد الزامی است.")]
        public string Day { get; set; }

        [Required(ErrorMessage = "وارد کردن این مورد الزامی است.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "تنها می توانید از اعداد استفاده نمایید.")]
        [Range(0, 23, ErrorMessage = "ساعت باید عددی بین 0 تا 23 باشد.")]
        public int Hour { get; set; }

        [Required(ErrorMessage = "وارد کردن این مورد الزامی است.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "تنها می توانید از اعداد استفاده نمایید.")]
        [Range(0, 59, ErrorMessage = "دقیقه باید عددی بین 0 تا 59 باشد.")]
        public int Minute { get; set; }

        public bool Reserved { get; set; }
        public bool Taken { get; set; }

        public virtual Meeting Meeting { get; set; }
    }
}
