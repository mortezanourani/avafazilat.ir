using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsPassword
{
    public string Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
