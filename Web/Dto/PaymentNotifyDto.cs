﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class PaymentNotifyDto
    {
        public string transactionId { get; set; }
        public string token { get; set; }
        public string hash { get; set; }
    }
}
