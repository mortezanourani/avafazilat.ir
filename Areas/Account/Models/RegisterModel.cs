using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "پست الکترونیک را به صورت صحیح وارد نمایید.")]
        [Display(Name = "پست الکترونیک")]
        public string Email { get; set; }

        [Required]
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
