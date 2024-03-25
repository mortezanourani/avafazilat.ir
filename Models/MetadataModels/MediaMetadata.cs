using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class MediaMetadata
{
    [Display(Name = "نام فایل")]
    public string FileName { get; set; }

    [Display(Name = "پسوند فایل")]
    public string Extension { get; set; }

    [Display(Name = "آلبوم")]
    public Guid CategoryId { get; set; }

    [Display(Name = "تاریخ آپلود")]
    public string Uploaded { get; set; }

    [Display(Name = "آلبوم")]
    public virtual Category Category { get; set; }
}

[ModelMetadataType(typeof(MediaMetadata))]
public partial class Media
{
}
