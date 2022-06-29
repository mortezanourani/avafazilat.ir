using System.Collections.Generic;
using Fazilat.Models;
using Fazilat.Data;
#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class AdviserModel
    {
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<ApplicationUser> Advisers { get; set; }
    }
}
