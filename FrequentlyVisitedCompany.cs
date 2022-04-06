using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API
{
    public partial class FrequentlyVisitedCompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public DateTime VisitDate { get; set; }
        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
    }
}
