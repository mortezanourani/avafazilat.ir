using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fazilat.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }

    [PersonalData]
    public string LastName { get; set; }

    [PersonalData]
    public string BirthDate { get; set; }

    [PersonalData]
    public string Registered { get; }
}
