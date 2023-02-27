using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class CompanySales
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }

    }
}
