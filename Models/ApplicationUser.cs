using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "کد ملی")]
        public override string UserName { get; set; }
        [Display(Name = "شماره تماس")]
        public override string PhoneNumber { get; set; }
        public virtual UserInformation Information { get; set; }
        public virtual EducationalFile EducationalFile { get; set; }
        public virtual ICollection<FinancialRecord> FinancialFile { get; set; }
        public virtual ICollection<Curriculum> Curriculums { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
