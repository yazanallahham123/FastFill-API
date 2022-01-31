using System;
using System.Collections.Generic;

#nullable disable

namespace FastFill_API
{
    public partial class CompanyBranch
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Code { get; set; }
        public string ArabicAddress { get; set; }
        public string EnglishAddress { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public bool? IsOpen { get; set; }
        public bool? Disabled { get; set; }
        public string BankAccountId { get; set; }
        public bool? IsDedicatedBankAccount { get; set; }

        public virtual Company Company { get; set; }
    }
}
