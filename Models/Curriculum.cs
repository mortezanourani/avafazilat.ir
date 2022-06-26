using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Fazilat.Models
{
    public class Curriculum
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
