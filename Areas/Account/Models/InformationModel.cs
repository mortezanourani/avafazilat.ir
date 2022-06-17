using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Fazilat.Models;
using System;

#nullable disable
namespace Fazilat.Areas.Account.Models
{
    public class InformationModel : UserInformation
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "National code is invalid.")]
        [StringLength(10, ErrorMessage = "The {0} must has {1} digits.", MinimumLength = 10)]
        public string NationalCode { get; set; }

        [RegularExpression("^[آ-یای]+$", ErrorMessage = "Invalid first name.")]
        [StringLength(100, ErrorMessage = "The {0} must has {1} max character length.")]
        public string FirstName { get; set; }

        [RegularExpression("^[آ-یای]+$", ErrorMessage = "Invalid last name.")]
        [StringLength(100, ErrorMessage = "The {0} must has {1} max character length.")]
        public string LastName { get; set; }

        //public new DateTime? BirthDate { get; set; }

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
