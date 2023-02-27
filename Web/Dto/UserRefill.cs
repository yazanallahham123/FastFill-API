using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class UserRefill
    {
        public int? Id { get; set; }
        public string? transactionId { get; set; }
        public int? userId { get; set; }
        public double? amount { get; set; }
        public bool? status { get; set; }
        public string? mobileNumber { get; set; }
        public DateTime? date { get; set; }
        public string? source { get; set; }
        public int? sourceId { get; set; }
    }
}
