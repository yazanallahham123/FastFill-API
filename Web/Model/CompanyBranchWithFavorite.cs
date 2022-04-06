using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class CompanyBranchWithFavorite : CompanyBranch
    {
        public bool IsFavorite { get; set; }
    }
}
