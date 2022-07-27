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
        public string NationalCode { get; set; }

        [Display(Name = "نام")]
        [RegularExpression("^[آ-یای ]+$", ErrorMessage = "نام باید به صورت فارسی وارد گردد.")]
        [StringLength(100, ErrorMessage = "نام نمی تواند بیش از 100 حرف باشد.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [RegularExpression("^[آ-یای ]+$", ErrorMessage = "نام خانوادگی باید به صورت فارسی وارد گردد.")]
        [StringLength(100, ErrorMessage = "نام خانوادگی نمی تواند بیش از 100 حرف باشد.")]
        public string LastName { get; set; }

        [Display(Name = "تاریخ تولد")]
        public new DateTime? BirthDate { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(1, 31, ErrorMessage = "روز عددی بین 1 تا 31 است.")]
        public int Day { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(1, 12, ErrorMessage = "ماه عددی بین 1 تا 12 است.")]
        public int Month { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(1300, 1500, ErrorMessage = "سال را به صورت 4 رقمی وارد نمایید.")]
        public int Year { get; set; }

        [Display(Name = "شماره تماس")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [StringLength(11, ErrorMessage = "شماره تماس را به صورت 11 رقمی وارد نمایید.", MinimumLength = 11)]
        [Range(9000000000, 9399999999, ErrorMessage = "شماره تماس وارد شده نامعتبر می باشد.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "استان")]
        [RegularExpression("^[آ-یای ]+$", ErrorMessage = "نام استان باید به صورت فارسی وارد گردد.")]
        [StringLength(100, ErrorMessage = "نام استان نمی تواند بیش از 100 حرف باشد.", MinimumLength = 4)]
        public string Province { get; set; }

        [Display(Name = "تصویر شناسنامه")]
        public IFormFile BirthCertificateFile { get; set; }
    }
}
