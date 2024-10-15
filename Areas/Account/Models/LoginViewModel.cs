using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Account.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
}
