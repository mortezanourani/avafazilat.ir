using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsPassword
{
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "رمزعبور جدید و تکرار آن متفاوت است. لطفا در وارد نمودن رمزعبور جدید دقت نمایید.")]
    public string ConfirmNewPassword { get; set; }
}
