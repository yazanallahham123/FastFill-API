using System;
using System.Collections.Generic;

#nullable disable

namespace FastFill_API.Model
{
    public partial class Company
    {
        public Company()
        {
            CompanyBranches = new HashSet<CompanyBranch>();
        }

        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Code { get; set; }
        public string ArabicAddress { get; set; }
        public string EnglishAddress { get; set; }
        public bool? Disabled { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public bool? IsOpen { get; set; }
        public string BankAccountId { get; set; }
        public string LogoUrl { get; set; }

        public virtual ICollection<CompanyBranch> CompanyBranches { get; set; }
    }
}
