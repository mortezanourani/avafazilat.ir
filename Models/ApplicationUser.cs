using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "پست الکترونیک")]
        public string Email { get; set; }
        public virtual UserInformation Information { get; set; }
        public virtual EducationalFile EducationalFile { get; set; }
        public virtual ICollection<Curriculum> Curriculums { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
