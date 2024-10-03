using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fazilat.Models;

public class ApplicationUser : IdentityUser
{
    [NotMapped]
    public string FirstName { get; set; }

    [NotMapped]
    public string LastName { get; set; }

    [NotMapped]
    public string BirthDate { get; set; }

    [NotMapped]
    public string Expired { get; set; }

    [NotMapped]
    public string Expiration { get; set; }

    [PersonalData]
    public string Registered { get; set; }
}
