using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class UserMetadata
{
    [Display(Name = "کد ملی")]
    public string UserName { get; set; }

    [Display(Name = "شماره تماس")]
    public string PhoneNumber { get; set; }
}

[ModelMetadataType(typeof(UserMetadata))]
public partial class User : IdentityUser
{
}
