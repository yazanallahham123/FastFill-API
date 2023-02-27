using FastFill_API.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class RefillSourceAmount
    {
        public RefillSourceBody refillSource { get; set; }
        public double? successAmount { get; set; }
        public int? successCount { get; set; }
        public double? failAmount { get; set; }
        public int? failCount { get; set; }

    }
}
