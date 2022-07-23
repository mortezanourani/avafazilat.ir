using Fazilat.Models;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Account.Models
{
    public class CurriculumModel : Curriculum
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(1, 31, ErrorMessage = "Enter valid number.")]
        public int Day { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(1, 12, ErrorMessage = "Enter valid number.")]
        public int Month { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(1300, 1500, ErrorMessage = "Enter valid number.")]
        public int Year { get; set; }
    }
}
