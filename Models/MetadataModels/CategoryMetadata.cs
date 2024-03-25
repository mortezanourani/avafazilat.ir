using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class CategoryMetadata
{
    [Display(Name = "عنوان دسته")]
    public string Name { get; set; }

    [Display(Name = "عنوان فارسی دسته")]
    public string PersianName { get; set; }

    public virtual ICollection<Media> Media { get; set; } = new List<Media>();
}

[ModelMetadataType(typeof(CategoryMetadata))]
public partial class Category
{
}
