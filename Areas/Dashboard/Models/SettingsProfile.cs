using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsProfile
{
    [Required(ErrorMessage = "وارد کردن کد ملی الزامی است.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "وارد کردن نام الزامی است.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی است.")]
    public string LastName { get; set; }

    public string BirthDate { get; set; }
}
