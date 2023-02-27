using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class WeekTotalPaymentTransactions
    {
        public double Today { get; set; }
        public double OneDayBefore { get; set; }
        public double TwoDaysBefore { get; set; }
        public double ThreeDaysBefore { get; set; }
        public double FourDaysBefore { get; set; }
        public double FiveDaysBefore { get; set; }
        public double SixDaysBefore { get; set; }
    }
}
