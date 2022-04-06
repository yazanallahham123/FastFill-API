using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class UpdateUserProfileDto
    {
        public string MobileNumber { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
    }
}
