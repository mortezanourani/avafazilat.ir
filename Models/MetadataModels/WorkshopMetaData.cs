using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class WorkshopMetaData
{
    [Display(Name = "پایه تحصیلی")]
    public int Grade { get; set; }

    [Display(Name = "عنوان دوره")]
    public string Title { get; set; }

    [Display(Name = "هزینه دوره")]
    public string Cost { get; set; }

    [Display(Name = "ظرفیت دوره")]
    public int Capacity { get; set; }

    [Display(Name = "تاریخ آغاز دوره")]
    public string StartDate { get; set; }
}

[ModelMetadataType(typeof(WorkshopMetaData))]
public partial class Workshop
{
}
