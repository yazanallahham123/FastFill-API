using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class MonthlyPaymentTransaction
    {
        public int count { get; set; }
        public double amount { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
}
