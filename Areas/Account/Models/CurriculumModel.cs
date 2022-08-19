using Fazilat.Models;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Areas.Account.Models
{
    public class CurriculumModel : Curriculum
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا از اعداد انگلیسی استفاده نمایید.")]
        [Range(1, 31, ErrorMessage = "روز عددی بین 1 تا 31 است.")]
        public int Day { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا از اعداد انگلیسی استفاده نمایید.")]
        [Range(1, 12, ErrorMessage = "ماه عددی بین 1 تا 12 است.")]
        public int Month { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا از اعداد انگلیسی استفاده نمایید.")]
        [Range(1300, 1500, ErrorMessage = "سال را به صورت 4 رقمی وارد نمایید.")]
        public int Year { get; set; }
    }
}
