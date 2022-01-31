using FastFill_API.Web.Utils.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = LoginErrorMessages.PASSWORD_REQUIRED)]
        public string password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = LoginErrorMessages.PASSWORD_REQUIRED)]
        public string mobileNumber { get; set; }
    }
}
