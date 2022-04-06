using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class PaymentTransactionDto
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int CompanyId { get; set; }
        public int FuelTypeId { get; set; }
        public double Amount { get; set; }
        public double Fastfill { get; set; }
        public bool Status { get; set; }
    }
}
