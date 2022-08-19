using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Fazilat.Models;

#nullable disable
#pragma warning disable
namespace Fazilat.Areas.Account.Models
{
    public class EducationalFileModel : EducationalFile
    {
        [Display(Name = "مقطع تحصیلی")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا از اعداد انگلیسی استفاده نمایید.")]
        [StringLength(2, MinimumLength = 2)]
        [Range(10, 12, ErrorMessage = "پایه تحصیلی عددی بین 10 و 12 است.")]
        public string Grade { get; set; }

        [Display(Name = "آخرین معدل کارنامه")]
        [RegularExpression("^[0-9]{2}[.][0-9]{2}$", ErrorMessage = "معدل باید طبق الگوی 17.89  و با اعداد انگلیسی وارد شود.")]
        [StringLength(5, MinimumLength = 5)]
        public string LastAvg { get; set; }

        [Display(Name = "تصویر فرم ثبت نام")]
        public IFormFile RegistrationFormFile { get; set; }

        [Display(Name = "تصویر آخرین کارنامه تحصیلی")]
        public IFormFile LastWorkbookFile { get; set; }
    }
}
