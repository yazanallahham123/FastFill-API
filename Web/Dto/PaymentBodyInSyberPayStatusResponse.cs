using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class PaymentBodyInSyberPayStatusResponse
    {
        public string? status { get; set; }
        public string? serviceId { get; set; }
        public double? amount { get; set; }
        public string? currency { get; set; }
        public string? customerRef { get; set; }
        public string? tranTimestamp { get; set; }
        public string? responseMessage { get; set; }
        public string? transactionId { get; set; }
    }
}
