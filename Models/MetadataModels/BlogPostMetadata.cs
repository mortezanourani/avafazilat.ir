using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class BlogPostMetadata
{
    [Display(Name = "عنوان خبر")]
    public string Title { get; set; }

    [Display(Name = "تصویر")]
    public string Image { get; set; }

    [Display(Name = "متن خبر")]
    public string Content { get; set; }

    [Display(Name = "خبر نمایش داده شود؟")]
    public bool IsVisible { get; set; }
}

[ModelMetadataType(typeof(BlogPostMetadata))]
public partial class BlogPost
{
    public virtual IFormFile ImageFile { get; set; }
}
