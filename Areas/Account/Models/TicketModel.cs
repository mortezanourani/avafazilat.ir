using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Fazilat.Models;

#pragma warning disable
namespace Fazilat.Areas.Account.Models
{
    public class TicketModel : Ticket
    {
        public ICollection<Ticket> Tickets { get; set; }
    }
}
