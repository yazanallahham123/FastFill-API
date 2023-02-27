using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class UserRefillTransactionDto
    {
        public string transactionId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool status { get; set; }
        public int RefillSourceId { get; set; }
    }
}
