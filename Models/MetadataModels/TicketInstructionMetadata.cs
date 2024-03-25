using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models;

public class TicketInstructionMetadata
{
    [Required]
    [Display(Name = "سامانه نوبت دهی فعال بوده و نمایش داده شود؟")]
    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "عنوان موضوع سامانه نوبت دهی")]
    public string Title { get; set; }

    [Display(Name = "محتوای دستورالعمل درخواست نوبت جلسه مشاوره")]
    public string Content { get; set; }
}

[ModelMetadataType(typeof(TicketInstructionMetadata))]
public partial class TicketInstruction
{
}