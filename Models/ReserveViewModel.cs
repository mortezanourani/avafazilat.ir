using System.Collections.Generic;

namespace Fazilat.Models
{
    public partial class ReserveViewModel : Meeting
    {
        public virtual TicketInstruction Instruction { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
