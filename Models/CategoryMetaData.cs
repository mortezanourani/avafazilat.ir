using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models
{
    [ModelMetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {
    }

    public class CategoryMetaData
    {
        [Display(Name = "عنوان دسته")]
        public string Name { get; set; }

        [Display(Name = "عنوان فارسی دسته")]
        public string PersianName { get; set; }

        public virtual ICollection<Media> Media { get; set; } = new List<Media>();
    }
}
