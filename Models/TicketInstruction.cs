using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Fazilat.Models
{
    public class TicketInstruction
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "سامانه نوبت دهی فعال بوده و نمایش داده شود؟")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "عنوان موضوع سامانه نوبت دهی")]
        public string Title { get; set; }

        [Display(Name = "محتوای دستورالعمل درخواست نوبت جلسه مشاوره")]
        public string Content { get; set; }

    }
}
