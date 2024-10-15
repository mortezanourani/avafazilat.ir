namespace Fazilat.Areas.Dashboard.Models;

public class UserSettingsViewModel
{
    public string Id { get; set; }

    public SettingsProfile Profile { get; set; }

    public SettingsCommunication Communication { get; set; }

    public SettingsPassword Security { get; set; }
}
