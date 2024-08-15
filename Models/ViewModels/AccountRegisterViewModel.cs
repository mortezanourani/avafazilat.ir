using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models.ViewModels;

public class AccountRegisterViewModel
{
    [Required]
    [Display(Name = "نام")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "نام خانوداگی")]
    public string LastName { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "شماره همراه")]
    public string PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "تکرار رمز عبور نادرست است.")]
    [Display(Name = "تکرار رمز عبور")]
    public string ConfirmPassword { get; set; }
}
