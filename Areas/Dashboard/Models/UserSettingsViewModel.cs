namespace Fazilat.Areas.Dashboard.Models;

public class UserSettingsViewModel
{
    public SettingsProfile Profile { get; set; }

    public SettingsCommunication Communication { get; set; }

    public SettingsPassword Security { get; set; }
}
