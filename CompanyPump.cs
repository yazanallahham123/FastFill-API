using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class CompanyPump
    {
        public CompanyPump()
        {
            PaymentTransactions = new HashSet<PaymentTransaction>();
            Agents = new HashSet<CompanyAgentPump>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Company Company { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<CompanyAgentPump> Agents { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
