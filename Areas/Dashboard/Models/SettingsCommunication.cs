using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsCommunication
{
    public string Id { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
