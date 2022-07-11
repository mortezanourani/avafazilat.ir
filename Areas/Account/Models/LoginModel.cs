using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "نام کاربری")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
