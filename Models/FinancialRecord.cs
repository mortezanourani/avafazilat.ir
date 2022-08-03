using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models
{
    public class FinancialRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "وارد کردن کد رهگیری الزامی می باشد.")]
        [Display(Name = "کد رهگیری")]
        public string TrackingCode { get; set; }
        [Required(ErrorMessage = "وارد کردن موضوع مبلغ پرداختی الزامی می باشد.")]
        [Display(Name = "توضیح مبلغ پرداختی")]
        public string Description { get; set; }
        public string PaymentReceipt { get; set; }
        public bool IsApproved { get; set; }

        public virtual ApplicationUser User { get; set; }
        [Required(ErrorMessage = "ارسال تصویر رسید پرداخت شهریه الزامی می باشد.")]
        [Display(Name = "تصویر رسید پرداخت")]
        public virtual IFormFile PaymentReceiptFile { get; set; }
    }
}
