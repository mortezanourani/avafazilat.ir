using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models.ViewModels;

public class AccountSignInViewModel
{
    [Required]
    [Display(Name = "نام کاربری")]
    public string Username { get; set; }

    [Required]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }
}
