using System;
using System.Collections.Generic;

#nullable disable

namespace FastFill_API
{
    public partial class TransactionType
    {
        public TransactionType()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
