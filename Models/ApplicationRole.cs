using Microsoft.AspNetCore.Identity;

namespace Fazilat.Models
{
    public class ApplicationRole : IdentityRole
    {
        [PersonalData]
        public string PersianName { get; set; }
    }
}
