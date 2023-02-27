using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class PaymentTransactionResultsFilter
    {
        public string? mobileNumber { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public List<int>? companies { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
