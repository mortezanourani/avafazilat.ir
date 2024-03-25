using System;
using System.Collections.Generic;

namespace Fazilat.Models;

public partial class Course
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Topics { get; set; }

    public bool Accomplished { get; set; }

    public string Descritpion { get; set; }

    public string CurriculumId { get; set; }

    public virtual Curriculum Curriculum { get; set; }
}
