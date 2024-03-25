using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class MeetingMetadata
{
    [Display(Name = "نام و نام خانوادگی")]
    [Required(ErrorMessage = "وارد کردن نام و نام خانوادگی الزامی می باشد.")]
    [RegularExpression("^[آ-یای ]+$", ErrorMessage = "نام و نام خانوادگی را به فارسی و به صورت کامل وارد نمایید.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "این مورد الزامی است.")]
    [Display(Name = "شماره موبایل")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا از اعداد انگلیسی استفاده نمایید.")]
    [StringLength(11, ErrorMessage = "شماره تماس را به صورت 11 رقمی وارد نمایید.", MinimumLength = 11)]
    [Range(9000000000, 9999999999, ErrorMessage = "شماره تماس وارد شده نا معتبر می باشد.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "این مورد الزامی است.")]
    [Display(Name = "رشته تحصیلی")]
    public string Major { get; set; }

    [Required(ErrorMessage = "این مورد الزامی است.")]
    [Display(Name = "حضوری یا مجازی")]
    public string Type { get; set; }

    [Required(ErrorMessage = "انتخاب تاریخ و زمان جلسه مشاوره الزامی است.")]
    [Display(Name = "ساعت و تاریخ جلسه مشاوره")]
    public virtual Ticket Ticket { get; set; }

    [Required(ErrorMessage = "ارسال تصویر فیش واریزی الزامی است.")]
    [Display(Name = "تصویر فیش واریز وجه")]
    public virtual IFormFile PaymentFile { get; set; }
}

[ModelMetadataType(typeof(MeetingMetadata))]
public partial class Meeting
{
    public virtual IFormFile PaymentFile { get; set; }
}