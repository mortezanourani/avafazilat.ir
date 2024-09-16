using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models.ViewModels;

public class AccountViewModel
{
    public bool isLoginPage { get; set; }
    public AccountLoginViewModel Login { get; set; }
    public AccountRegisterViewModel Register { get; set; }
}
