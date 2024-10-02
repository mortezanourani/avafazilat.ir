using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Account.Models;

public class AccountViewModel
{
    public bool isLoginPage { get; set; }
    public LoginViewModel Login { get; set; }
    public RegisterViewModel Register { get; set; }
}
