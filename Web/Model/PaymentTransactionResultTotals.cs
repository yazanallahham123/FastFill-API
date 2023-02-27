using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class PaymentTransactionResultTotals
    {
        public double? TotalAmount { get; set; }
        public double? TotalFastfill { get; set; }
        public int? TotalCount { get; set; }
    }
}
