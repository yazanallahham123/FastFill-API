using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class CancelPaymentDto
    {
        public string customerRef { get; set; }
        public string responseStatus { get; set; }
    }
}
