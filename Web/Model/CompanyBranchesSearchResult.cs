using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class CompanyBranchesSearchResult
    {
        List<CompanyBranch> FavoriteCompaniesBranches { get; set; }
        List<CompanyBranch> FrequenltyVisitedCompaniesBranches { get; set; }
    }
}
