using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Fazilat.Models;

#nullable disable
#pragma warning disable
namespace Fazilat.Areas.Account.Models
{
    public class EducationalFileModel : EducationalFile
    {
        [Display(Name = "Grade")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [StringLength(2, MinimumLength = 2)]
        [Range(10, 12, ErrorMessage = "Enter valid grade.")]
        public string Grade { get; set; }

        [Display(Name = "Last Avarage")]
        [RegularExpression("^[0-9]{2}[.][0-9]{2}$", ErrorMessage = "The average must has 17.89 pattern.")]
        [StringLength(5, MinimumLength = 5)]
        public string LastAvg { get; set; }

        [Display(Name = "Registration Form")]
        public IFormFile RegistrationFormFile { get; set; }

        [Display(Name = "Last Workbook")]
        public IFormFile LastWorkbookFile { get; set; }
    }
}
