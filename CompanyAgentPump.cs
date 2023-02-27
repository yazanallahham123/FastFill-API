using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class CompanyAgentPump
    {

        public CompanyAgentPump()
        {
            States = new HashSet<CompanyPumpState>();
        }

        public int Id { get; set; }
        public int AgentId { get; set; }
        public int PumpId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual CompanyPump Pump { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual User Agent { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<CompanyPumpState> States { get; set; }


    }
}
