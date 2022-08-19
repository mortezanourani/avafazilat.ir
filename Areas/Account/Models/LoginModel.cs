using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "وارد کردن کد ملی یا شماره همراه الزامی است.")]
        [Display(Name = "کد ملی یا شماره همراه")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا نام کاربری را به  صورت درست و با اعداد انگلیسی وارد نمایید.")]
        [StringLength(11, ErrorMessage = "تعداد ارقام وارد شده غیر مجاز است.", MinimumLength = 10)]
        public string Username { get; set; }

        [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
