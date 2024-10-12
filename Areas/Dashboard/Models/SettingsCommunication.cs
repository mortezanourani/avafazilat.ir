using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsCommunication
{
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}
