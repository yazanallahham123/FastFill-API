using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Model
{
    public class Totals
    {
        public int Count { get; set; }
        public double Amount { get; set; }

        public void SetValues(int count, double amount)
        {

            this.Amount = amount;
            this.Count = count;
        }
    }
}
