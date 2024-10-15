using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Dashboard.Models;

public class SettingsCommunication
{
    [Required(ErrorMessage = "شماره تلفن همراه الزامی است.")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "لطفا پست الکترونیک خود را به درستی وارد نمایید.")]
    public string Email { get; set; }
}
