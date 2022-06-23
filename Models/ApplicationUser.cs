using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

#nullable disable
namespace Fazilat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserInformation Information { get; set; }
        public virtual EducationalFile EducationalFile { get; set; }
        public virtual ICollection<Curriculum> Curriculums { get; set; }
    }
}
