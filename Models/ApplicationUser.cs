using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fazilat.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }

    [PersonalData]
    public string LastName { get; set; }

    [PersonalData]
    public DateTime? BirthDate { get; set; }

    [NotMapped]
    public string Expired { get; set; }

    [NotMapped]
    public string Expiration { get; set; }

    [PersonalData]
    public DateTime Registered { get; set; }
}
