﻿using Fazilat.Models;
using System.Collections.Generic;

namespace Fazilat.Areas.Dashboard.Models;

public class HomeModel
{
    public ApplicationRole Panel { get; set; }

    public ApplicationUser User { get; set; }

    public ICollection<ApplicationUser> Users { get; set; }

    public ICollection<Province> Provinces { get; set; }

    public ICollection<ApplicationRole> UserRoles { get; set; }
}
