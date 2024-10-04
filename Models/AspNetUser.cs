using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class AspNetUser
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string UserName { get; set; }

    public string NormalizedUserName { get; set; }

    public string Email { get; set; }

    public string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<ActivityReport> ActivityReports { get; set; } = new List<ActivityReport>();

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Schedule> ScheduleCounsellors { get; set; } = new List<Schedule>();

    public virtual ICollection<Schedule> ScheduleUsers { get; set; } = new List<Schedule>();

    public virtual ICollection<SleepReport> SleepReports { get; set; } = new List<SleepReport>();

    public virtual ICollection<AspNetUser> Counsellors { get; set; } = new List<AspNetUser>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<AspNetUser> Parents { get; set; } = new List<AspNetUser>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();

    public virtual ICollection<AspNetUser> UsersNavigation { get; set; } = new List<AspNetUser>();
}
