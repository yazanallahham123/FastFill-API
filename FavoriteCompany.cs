using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#nullable disable

namespace FastFill_API
{
    public class FavoriteCompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]

        public virtual Company Company { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]

        public virtual User User { get; set; }
    }
}
