using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Fazilat.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Topics { get; set; }
        public bool Accomplished { get; set; }
        public string Descritpion { get; set; }
        public string CurriculumId { get; set; }
        public virtual Curriculum Curriculum { get; set; }
    }
}
