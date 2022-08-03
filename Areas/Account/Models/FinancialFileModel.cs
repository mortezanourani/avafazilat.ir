using Fazilat.Models;
using System.Collections.Generic;

namespace Fazilat.Areas.Account.Models
{
    public class FinancialFileModel : FinancialRecord
    {
        public virtual ICollection<FinancialRecord> FinancialRecords { get; set; }
    }
}
