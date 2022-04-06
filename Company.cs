using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#nullable disable

namespace FastFill_API
{
    public partial class Company
    {
        public Company()
        {
            CompanyBranches = new HashSet<CompanyBranch>();
            FavoriteCompanies = new HashSet<FavoriteCompany>();
            FrequentlyVisitedCompanies = new HashSet<FrequentlyVisitedCompany>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
            Users = new HashSet<User>();
            Notifications = new HashSet<Notification>();
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

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<CompanyBranch> CompanyBranches { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]

        public virtual ICollection<FavoriteCompany> FavoriteCompanies { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]

        public virtual ICollection<FrequentlyVisitedCompany> FrequentlyVisitedCompanies { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<User> Users { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
