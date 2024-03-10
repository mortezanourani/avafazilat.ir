using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Account.Models
{
    public partial class Login
    {
        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
    }
}
