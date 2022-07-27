using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Models
{
    public class Meeting
    {
        public string Id { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن نام و نام خانوادگی الزامی می باشد.")]
        [RegularExpression("^[آ-یای ]+$", ErrorMessage = "نام و نام خانوادگی را به فارسی و به صورت کامل وارد نمایید.")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "شماره موبایل")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [StringLength(11, ErrorMessage = "The {0} must has {1} max character length.", MinimumLength = 11)]
        [Range(9000000000, 9399999999, ErrorMessage = "Enter valid number.")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "رشته تحصیلی")]
        public string Major { get; set; }
        [Required]
        [Display(Name = "حضوری یا مجازی")]
        public string Type { get; set; }
        [Required]
        public byte[] Payment { get; set; }
        public string TicketId { get; set; }
        public bool Confirmed { get; set; }

        [Required]
        [Display(Name = "تصویر فیش واریز وجه")]
        public virtual IFormFile PaymentFile { get; set; }
        [Required]
        [Display(Name = "ساعت و تاریخ جلسه مشاوره")]
        public virtual Ticket Ticket { get; set; }
    }
}
