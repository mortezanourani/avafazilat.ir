﻿using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class UserLimitation
{
    public string UserId { get; set; }

    public DateOnly Expiration { get; set; }

    public virtual User User { get; set; }
}
