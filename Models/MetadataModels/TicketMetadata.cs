using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class TicketMetadata
{
    [Display(Name = "نوبت مشاوره انتخاب رشته")]
    [Required(ErrorMessage = "وارد کردن این مورد الزامی است.")]
    public string Day { get; set; }

    [Required(ErrorMessage = "وارد کردن این مورد الزامی است.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا از اعداد انگلیسی استفاده نمایید.")]
    [Range(0, 23, ErrorMessage = "ساعت باید عددی بین 0 تا 23 باشد.")]
    public int Hour { get; set; }

    [Required(ErrorMessage = "وارد کردن این مورد الزامی است.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا از اعداد انگلیسی استفاده نمایید.")]
    [Range(0, 59, ErrorMessage = "دقیقه باید عددی بین 0 تا 59 باشد.")]
    public int Minute { get; set; }
}

[ModelMetadataType(typeof(Ticket))]
public partial class Ticket
{
}