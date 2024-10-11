using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsProfile
{
    public string Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string BirthDate { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Description = "رمزعبور فعلی")]
    public string Password { get; set; }
}
