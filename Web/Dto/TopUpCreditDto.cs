﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class TopUpCreditDto
    {
        public int? walletId { get; set; }
        public string? lang { get; set; }
        public string mobileNumber { get; set; }
        public double amount { get; set; }
        public string transactionId { get; set; }
        public string? token { get; set; }
    }
}
