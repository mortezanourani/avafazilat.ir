using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class UserEducationalFile
{
    public string UserId { get; set; }

    public string Grade { get; set; }

    public string LastAvg { get; set; }

    public string RegistrationFormFileName { get; set; }

    public string LastWorkbookFileName { get; set; }

    public virtual User User { get; set; }
}
