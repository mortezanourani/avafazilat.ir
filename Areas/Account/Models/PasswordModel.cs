using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class PasswordModel
    {
        [Required(ErrorMessage = "وارد کردن رمز عبور فعلی الزامی است.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "وارد کردن رمز عبور جدید الزامی است.")]
        [StringLength(100, ErrorMessage = "رمز عبور باید یک رشته از حروف و اعداد با طول حداقل 6 کارکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور جدید")]
        [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور با رمز عبور تفاوت دارد.")]
        public string ConfirmPassword { get; set; }
    }
}
