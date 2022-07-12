using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Fazilat.Models;
using System;

#nullable disable
#pragma warning disable
namespace Fazilat.Areas.Account.Models
{
    public class InformationModel : UserInformation
    {
        [Display(Name = "کد ملی")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "National code is invalid.")]
        [StringLength(10, ErrorMessage = "The {0} must has {1} digits.", MinimumLength = 10)]
        public string NationalCode { get; set; }

        [Display(Name = "نام")]
        [RegularExpression("^[آ-یای]+$", ErrorMessage = "Invalid first name.")]
        [StringLength(100, ErrorMessage = "The {0} must has {1} max character length.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [RegularExpression("^[آ-یای]+$", ErrorMessage = "Invalid last name.")]
        [StringLength(100, ErrorMessage = "The {0} must has {1} max character length.")]
        public string LastName { get; set; }

        [Display(Name = "تاریخ تولد")]
        public new DateTime? BirthDate { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(1, 31, ErrorMessage = "Enter valid number.")]
        public int Day { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(1, 12, ErrorMessage = "Enter valid number.")]
        public int Month { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(1300, 1500, ErrorMessage = "Enter valid number.")]
        public int Year { get; set; }

        [Display(Name = "شماره تماس")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [StringLength(11, ErrorMessage = "The {0} must has {1} max character length.", MinimumLength = 11)]
        [Range(9000000000, 9399999999, ErrorMessage = "Enter valid number.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "استان")]
        [RegularExpression("^[آ-یای]+$", ErrorMessage = "Invalid province name.")]
        [StringLength(100, ErrorMessage = "The {0} must has {1} max character length.", MinimumLength = 4)]
        public string Province { get; set; }

        [Display(Name = "تصویر شناسنامه")]
        public IFormFile BirthCertificateFile { get; set; }
    }
}
