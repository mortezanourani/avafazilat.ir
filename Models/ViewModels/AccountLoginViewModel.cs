using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models.ViewModels;

public class AccountLoginViewModel
{
    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
}
