using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Fazilat.Models;

namespace Fazilat.Areas.Account.Models
{
    public class TicketModel : Ticket
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

        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(0, 23, ErrorMessage = "Enter valid number.")]
        public int Hour { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter valid number.")]
        [Range(0, 59, ErrorMessage = "Enter valid number.")]
        public int Minute { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
