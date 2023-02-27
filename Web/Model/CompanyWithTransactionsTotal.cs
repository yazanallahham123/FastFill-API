using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class CompanyWithTransactionsTotal : Company
    {
        public int Count { get; set; }
        public double Amount { get; set; }
    }
}
