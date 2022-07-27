using System;
using System.ComponentModel.DataAnnotations;

namespace Fazilat.Models
{
    public class Ticket
    {
        public string Id { get; set; }
        [Display(Name = "تاریخ روز مشاوره")]
        public DateTime Time { get; set; }
        public bool Reserved { get; set; }
        public bool Taken { get; set; }

        public virtual Meeting Meeting { get; set; }
    }
}
