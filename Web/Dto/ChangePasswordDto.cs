using FastFill_API.Web.Utils.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class ChangePasswordDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = LoginErrorMessages.OLD_PASSWORD_REQUIRED)]
        public string OldPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = LoginErrorMessages.PASSWORD_REQUIRED)]
        public string NewPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = LoginErrorMessages.MOBILE_NUMBER_REQUIRED)]
        public string MobileNumber { get; set; }
    }
}
