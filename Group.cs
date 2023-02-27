using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#nullable disable

namespace FastFill_API
{
    public partial class Group
    {
        public Group()
        {
            Companies = new HashSet<Company>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }


        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Company> Companies { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<User> Users { get; set; }

    }
}
