using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fazilat.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Display(Name = "نام")]
    public string FirstName { get; set; }

    [PersonalData]
    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; }

    [PersonalData]
    [Display(Name = "تاریخ تولد")]
    public DateTime? BirthDate { get; set; }

    [NotMapped]
    public string Expired { get; set; }

    [NotMapped]
    [Display(Name = "تاریخ اتمام عضویت")]
    public string Expiration { get; set; }

    [PersonalData]
    [Display(Name = "تاریخ ثبت نام")]
    public DateTime Registered { get; set; }
}
