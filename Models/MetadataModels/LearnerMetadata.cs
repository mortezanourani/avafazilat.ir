using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models
{
    public class LearnerMetadata
    {
        [Required(ErrorMessage = "وارد کردن نام و نام خانوادگی الزامیست.")]
        [Display(Name = "نام و نام خانوادگی")]
        public string Name { get; set; }

        [Required(ErrorMessage = "وارد کردن شماره تماس الزامیست.")]
        [Display(Name = "شماره تماس دانش آموز")]
        public string Phone { get; set; }

        [Display(Name = "شماره تماس والدین")]
        public string ParentPhone { get; set; }

        [Required(ErrorMessage = "وارد کردن محل سکونت الزامیست.")]
        [Display(Name = "شهر محل سکونت")]
        public string City { get; set; }

        [Required]
        [Display(Name = "منطقه")]
        public string District { get; set; }

        [Required(ErrorMessage = "وارد کردن نام مدرسه محل تحصیل الزامیست.")]
        [Display(Name = "نام مدرسه")]
        public string School { get; set; }

        [Required(ErrorMessage = "لطفا کد رهگیری مربوط به پرداخت مجموع هزینه کلاس ها را وارد نمایید.")]
        [Display(Name = "کد رهگیری تراکنش بانکی")]
        public string TrackingCode { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public string Registered { get; set; }
    }

    [ModelMetadataType(typeof(LearnerMetadata))]
    public partial class Learner
    {
    }
}
