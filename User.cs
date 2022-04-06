using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#nullable disable

namespace FastFill_API
{
    public partial class User
    {
        public User()
        {
            Wallets = new HashSet<Wallet>();
            FavoriteCompanies = new HashSet<FavoriteCompany>();
            FavoriteCompaniesBranches = new HashSet<FavoriteCompanyBranch>();
            FrequentlyVisitedCompanies = new HashSet<FrequentlyVisitedCompany>();
            FrequentlyVisitedCompaniesBranches = new HashSet<FrequentlyVisitedCompanyBranch>();
            Notifications = new HashSet<Notification>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
            UserCredits = new HashSet<UserCredit>();
            BankCards = new HashSet<BankCard>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public string MobileNumber { get; set; }
        public bool? Disabled { get; set; }
        public byte[] StoredSalt { get; set; }

        public int? CompanyId { get; set; }

        public string? FirebaseToken { get; set; }
        public string ImageURL { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual Company? Company { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
        public virtual ICollection<FavoriteCompany> FavoriteCompanies { get; set; }
        public virtual ICollection<FavoriteCompanyBranch> FavoriteCompaniesBranches { get; set; }
        public virtual ICollection<FrequentlyVisitedCompany> FrequentlyVisitedCompanies { get; set; }
        public virtual ICollection<FrequentlyVisitedCompanyBranch> FrequentlyVisitedCompaniesBranches { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public virtual ICollection<UserCredit> UserCredits { get; set; }
        public virtual ICollection<BankCard> BankCards { get; set; }
    }
}
