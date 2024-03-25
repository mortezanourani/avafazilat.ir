using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class UserInformation
{
    public string UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string Province { get; set; }

    public byte[] BirthCertificate { get; set; }

    public virtual User User { get; set; }
}
