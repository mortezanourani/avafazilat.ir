using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Account.Models;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور کنونی")]
    public string OldPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور جدید")]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور درست نیست.")]
    [Display(Name = "تکرار رمز عبور جدید")]
    public string NewPasswordRetype { get; set; }
}
