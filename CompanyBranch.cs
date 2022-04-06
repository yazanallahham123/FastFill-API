using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#nullable disable

namespace FastFill_API
{
    public partial class CompanyBranch
    {
        public CompanyBranch()
        {
            FavoriteCompaniesBranches = new HashSet<FavoriteCompanyBranch>();
            FrequentlyVisitedCompaniesBranches = new HashSet<FrequentlyVisitedCompanyBranch>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
        }

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

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Company Company { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<FavoriteCompanyBranch> FavoriteCompaniesBranches { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<FrequentlyVisitedCompanyBranch> FrequentlyVisitedCompaniesBranches { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }

    }
}
