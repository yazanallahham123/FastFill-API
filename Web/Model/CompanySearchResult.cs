using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class CompanySearchResult
    {
        List<Company> FavoriteCompanies { get; set; }
        List<Company> FrequenltyVisitedCompanies { get; set; }
    }
}
