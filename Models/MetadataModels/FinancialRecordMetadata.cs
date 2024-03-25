using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class FinancialRecordMetadata
{
    [Required(ErrorMessage = "وارد کردن کد رهگیری الزامی می باشد.")]
    [Display(Name = "کد رهگیری")]
    public string TrackingCode { get; set; }

    [Required(ErrorMessage = "وارد کردن موضوع مبلغ پرداختی الزامی می باشد.")]
    [Display(Name = "توضیح مبلغ پرداختی")]
    public string Description { get; set; }
}

[ModelMetadataType(typeof(FinancialRecordMetadata))]
public partial class FinancialRecord
{
    [Required(ErrorMessage = "ارسال تصویر رسید پرداخت شهریه الزامی می باشد.")]
    [Display(Name = "تصویر رسید پرداخت")]
    public virtual IFormFile PaymentReceiptFile { get; set; }
}
