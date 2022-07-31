using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "وارد کردن کد ملی الزامی است.")]
        [Display(Name = "کد ملی")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا کد ملی خود را به شکل درستی وارد نمایید.")]
        [StringLength(10, ErrorMessage = "کد ملی می باید به صورت 10 رقمی و بدون خط فاصله وارد شود.", MinimumLength = 10)]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "وارد کردن شماره همراه الزامی است.")]
        [Display(Name = "شماره همراه")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "تنها می توانید از اعداد استفاده نمایید.")]
        [StringLength(11, ErrorMessage = "شماره همراه باید به صورت 11 رقمی و بدون کد کشور وارد شود.", MinimumLength = 11)]
        [Range(9000000000, 9399999999, ErrorMessage = "شماره همراه وارد شده نامعتبر است.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است.")]
        [StringLength(100, ErrorMessage = "رمز عبور باید رشته ای از اعداد و حروف با طول حداقل 6 کارکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن یکسان نیستند.")]
        public string ConfirmPassword { get; set; }
    }
}
