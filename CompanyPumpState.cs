using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class CompanyPumpState
    {

        public int Id { get; set; }
        public int CompanyAgentPumpId { get; set; }
        public bool IsOpen { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }


        [JsonIgnore]
        [IgnoreDataMember]
        public virtual CompanyAgentPump CompanyAgentPump { get; set; }
    }
}
