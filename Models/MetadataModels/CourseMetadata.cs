using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class CourseMetadata
{
    [Required(ErrorMessage = "این مورد الزامی است.")]
    [Display(Name = "عنوان درس")]
    [RegularExpression("^[آ-یای ]+$", ErrorMessage = "لطفا به صورت فارسی وارد گردد.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "این مورد الزامی است.")]
    [Display(Name = "سرفصل ها")]
    [RegularExpression("^[آ-یای ]+$", ErrorMessage = "لطفا به صورت فارسی وارد گردد.")]
    public string Topics { get; set; }

    [Display(Name = "کامل مطالعه نمودم.")]
    public bool Accomplished { get; set; }

    [Display(Name = "توضیح چگونگی اجرا")]
    public string Descritpion { get; set; }
}

[ModelMetadataType(typeof(CourseMetadata))]
public partial class Course
{
}
