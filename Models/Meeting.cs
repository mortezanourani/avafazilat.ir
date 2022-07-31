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

        [Required(ErrorMessage = "این مورد الزامی است.")]
        [Display(Name = "شماره موبایل")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "تنها می توانید از اعداد استفاده نمایید.")]
        [StringLength(11, ErrorMessage = "شماره تماس را به صورت 11 رقمی وارد نمایید.", MinimumLength = 11)]
        [Range(9000000000, 9399999999, ErrorMessage = "شماره تماس وارد شده نا معتبر می باشد.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "این مورد الزامی است.")]
        [Display(Name = "رشته تحصیلی")]
        public string Major { get; set; }

        [Required(ErrorMessage = "این مورد الزامی است.")]
        [Display(Name = "حضوری یا مجازی")]
        public string Type { get; set; }

        public byte[] Payment { get; set; }
        public string TicketId { get; set; }
        public bool Confirmed { get; set; }

        [Required(ErrorMessage = "ارسال تصویر فیش واریزی الزامی است.")]
        [Display(Name = "تصویر فیش واریز وجه")]
        public virtual IFormFile PaymentFile { get; set; }

        [Required(ErrorMessage = "انتخاب تاریخ و زمان جلسه مشاوره الزامی است.")]
        [Display(Name = "ساعت و تاریخ جلسه مشاوره")]
        public virtual Ticket Ticket { get; set; }
    }
}
