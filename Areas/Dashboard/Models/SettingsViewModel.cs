using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsViewModel
{
    public SettingsProfile Profile { get; set; }

    public SettingsCommunication Communication { get; set; }

    public SettingsPassword Security { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
