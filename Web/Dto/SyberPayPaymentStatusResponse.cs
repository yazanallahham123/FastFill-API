using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class SyberPayPaymentStatusResponse
    {
        public string? status { get; set; } 
        public string? applicationId { get; set; }
        public string? transactionId { get; set; }
        public string? tranTimestamp { get; set; }
        public int? responseCode { get; set; }
        public string? responseMessage { get; set; }
        public PaymentBodyInSyberPayStatusResponse? payment { get; set; }
    }
}
