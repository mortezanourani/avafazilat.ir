using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsProfile
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string BirthDate { get; set; }
}
