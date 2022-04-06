using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class FavoriteCompanyBranch
    {
        public int Id { get; set; }
        public int CompanyBranchId { get; set; }
        public int UserId { get; set; }
        public virtual CompanyBranch CompanyBranch { get; set; }
        public virtual User User { get; set; }
    }
}
