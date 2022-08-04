using System.Collections.Generic;

namespace Fazilat.Models
{
    public class ReserveViewModel : Meeting
    {
        public virtual TicketInstruction Instruction { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
