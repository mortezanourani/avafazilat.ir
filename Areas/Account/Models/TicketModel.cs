using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Fazilat.Models;

namespace Fazilat.Areas.Account.Models
{
    public class TicketModel : Ticket
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(1, 31, ErrorMessage = "روز عددی بین 1 تا 31 است.")]
        public int Day { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(1, 12, ErrorMessage = "ماه عددی بین 1 تا 12 است.")]
        public int Month { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(1300, 1500, ErrorMessage = "سال را به صورت 4 رقمی وارد نمایید.")]
        public int Year { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(0, 23, ErrorMessage = "ساعت باید عددی بین 0 تا 23 باشد.")]
        public int Hour { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا عدد درستی وارد نمایید.")]
        [Range(0, 23, ErrorMessage = "دقیقه باید عددی بین 0 تا 59 باشد.")]
        public int Minute { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
