using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class MonthlyPaymentTransactionListDto
    {
        public MonthlyPaymentTransactionDto monthlyPaymentTransactionDto { get; set; }
        public IEnumerable<PaymentTransaction> paymentTransactions { get; set; }
    }
}
