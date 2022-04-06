using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public string ImageURL { get; set; }
        public bool Disabled { get; set; }
        public string Password { get; set; }
    }
}
