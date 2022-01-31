using System;
using System.Collections.Generic;

#nullable disable

namespace FastFill_API
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public Guid LongId { get; set; }
        public int WalletId { get; set; }
        public Guid WalletLongId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionValue { get; set; }
        public int TransactionType { get; set; }
        public string Description { get; set; }
        public int? TransactionReferenceId { get; set; }

        public virtual TransactionType TransactionTypeNavigation { get; set; }
        public virtual Wallet Wallet { get; set; }
        public virtual Wallet WalletLong { get; set; }
    }
}
