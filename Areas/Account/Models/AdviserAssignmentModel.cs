using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fazilat.Models;

namespace Fazilat.Areas.Account.Models
{
    public class AdviserAssignmentModel : Adviser
    {
        [Display(Name = "داوطلب")]
        public virtual ApplicationUser Student { get; set; }
        public virtual ApplicationUser Adviser { get; set; }
        [Display(Name = "مشاور")]
        public virtual ICollection<ApplicationUser> Advisers { get; set; }
    }
}
