using System;
using System.Collections.Generic;

#nullable disable

namespace FastFill_API.Model
{
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
