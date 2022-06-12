using Microsoft.AspNetCore.Identity;

#nullable disable
namespace Fazilat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserInformation Information { get; set; }
    }
}
