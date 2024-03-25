using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;

namespace Fazilat.Models;

public class CurriculumMetadata
{
    [Required(ErrorMessage = "این مورد الزامی است.")]
    [Display(Name = "عنوان برنامه")]
    [RegularExpression("^[آ-یای ]+$", ErrorMessage = "لطفا به صورت فارسی وارد گردد.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "این مورد الزامی است.")]
    [Display(Name = "تاریخ آغاز برنامه")]
    public DateTime StartDate { get; set; }

    [Display(Name = "توضیحات برنامه هفتگی")]
    [RegularExpression("^[آ-یای ]+$", ErrorMessage = "لطفا به صورت فارسی وارد گردد.")]
    public string Description { get; set; }
}

[ModelMetadataType(typeof(CurriculumMetadata))]
public partial class Curriculum
{
}
