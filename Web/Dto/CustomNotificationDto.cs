using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class CustomNotificationDto
    {
        public string mobileNumber { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }
}
