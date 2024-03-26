using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fazilat.Models;
using Microsoft.AspNetCore.Identity;

namespace Fazilat.Areas.Account.Models
{
    public class AdviserAssignmentModel : Adviser
    {
        [Display(Name = "داوطلب")]
        public virtual IdentityUser Student { get; set; }
        public virtual IdentityUser Adviser { get; set; }
        [Display(Name = "مشاور")]
        public virtual ICollection<IdentityUser> Advisers { get; set; }
    }
}
