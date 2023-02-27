using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class UserRefillFilter
    {
        public string? mobileNumber { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string? transactionId { get; set; }
        public bool? status { get; set; }
        public List<int>? refillSources { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
