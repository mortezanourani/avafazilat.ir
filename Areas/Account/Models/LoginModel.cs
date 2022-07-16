using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "کد ملی یا شماره همراه")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا نام کاربری را به شکل درستی وارد نمایید.")]
        [StringLength(11, ErrorMessage = "تعداد ارقام وارد شده غیر مجاز است.", MinimumLength = 10)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
