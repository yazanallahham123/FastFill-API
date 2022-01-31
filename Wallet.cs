using System;
using System.Collections.Generic;

#nullable disable

namespace FastFill_API
{
    public partial class Wallet
    {
        public Wallet()
        {
            TransactionWalletLongs = new HashSet<Transaction>();
            TransactionWallets = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public Guid LongId { get; set; }
        public int UserId { get; set; }
        public string LoginKey { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Transaction> TransactionWalletLongs { get; set; }
        public virtual ICollection<Transaction> TransactionWallets { get; set; }
    }
}
