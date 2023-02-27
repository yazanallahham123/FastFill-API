using FastFill_API.Web.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class RefillsTotals
    {
        public double? TotalAmount { get; set; }
        public int? TotalCount { get; set; }
        public double? TotalSuccessAmount { get; set; }
        public int? TotalSuccessCount { get; set; }
        public double? TotalFailAmount { get; set; }
        public int? TotalFailCount { get; set; }
        public List<RefillSourceAmount>? RefillSourceAmounts { get; set; }
    }
}
