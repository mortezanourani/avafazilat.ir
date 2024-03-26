using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Fazilat.Models;

namespace Fazilat.Areas.Account.Models
{
    public class RoleManagerModel
    {
        [Display(Name = "کاربر")]
        public IdentityUser User { get; set; }

        public string Role { get; set; }

        [Display(Name = "سطح دسترسی")]
        public ICollection<string> Roles { get; set; }
    }
}
