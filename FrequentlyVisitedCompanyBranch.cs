using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API
{
    public partial class FrequentlyVisitedCompanyBranch
    {
        public int Id { get; set; }
        public int CompanyBranchId { get; set; }
        public int UserId { get; set; }
        public DateTime VisitDate { get; set; }
        public virtual CompanyBranch CompanyBranch { get; set; }
        public virtual User User { get; set; }
    }
}
