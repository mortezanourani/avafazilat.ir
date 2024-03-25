using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;

namespace Fazilat.Models;

public class UserInformationMetadata
{
    [Display(Name = "نام")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; }

    [Display(Name = "تاریخ تولد")]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "استان محل سکونت")]
    public string Province { get; set; }

    [Display(Name = "نام و نام خانوادگی")]
    public virtual string FullName { get; }
}

[ModelMetadataType(typeof(UserInformationMetadata))]
public partial class UserInformation
{
    public virtual string FullName => FirstName + " " + LastName;
}
